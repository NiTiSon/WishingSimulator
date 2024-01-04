using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using static WishingSimulator.WellKnownWishingValues;

namespace WishingSimulator;

public sealed class Banner
{
	public BannerKind Kind { get; }
	private readonly Random rnd;

	private readonly IReadOnlySet<BannerEntry> entries;

	public Banner(BannerKind kind, bool appendDefaultItems, params BannerEntry[] entries)
	{
		rnd = new();
		Kind = kind;
		if (appendDefaultItems)
		{
			HashSet<BannerEntry> resultEntries = [];

			foreach (BannerEntry entry in Item.allItems)
				resultEntries.Add(entry);

			foreach (BannerEntry entry in entries)
			{
				resultEntries.RemoveWhere(e => e.Item == entry.Item); // Remove duplicates
				resultEntries.Add(entry);
			}


			this.entries = resultEntries;
		}
		else
		{
			this.entries = new HashSet<BannerEntry>(entries);
		}
	}

	static Banner()
	{
		RuntimeHelpers.RunClassConstructor(typeof(Characters).TypeHandle);
		RuntimeHelpers.RunClassConstructor(typeof(Items).TypeHandle);
	}

	public WishInfo Wish(Account account)
		=> Wish(account, DateTime.UtcNow);

	private WishInfo Wish(Account account, DateTime timeUTC)
	{
		double chance = rnd.NextDouble() * 100d; // Convert chance to procent chance

		uint sixPity = account.NextPity(this, 6);
		uint fivePity = account.NextPity(this, 5);
		uint fourPity = account.NextPity(this, 4);
		uint threePity = account.NextPity(this, 3);
		uint wishNumber = account.NextWishNumber(this);

		WishInfo wish;
		if (fivePity == FiveStarPity - 1 && fourPity == FourStarPity - 1)
		{
			WishInternal(account, 4, fourPity, wishNumber, timeUTC, out wish);
		}
		else if (fourPity == FourStarPity)
		{
			WishInternal(account, 4, fourPity, wishNumber, timeUTC, out wish);
		}
		else if (fivePity == FiveStarPity)
		{
			WishInternal(account, 5, fivePity, wishNumber, timeUTC, out wish);
		}
		else
		{
			if (chance <= SixStarChance) // six-star has no guarantees
			{
				WishInternalSixStar(sixPity, wishNumber, timeUTC, out wish);
			}
			else if (chance <= FiveStarChance)
			{
				WishInternal(account, 5, fivePity, wishNumber, timeUTC, out wish);
			}
			else if (chance <= FourStarChance)
			{
				WishInternal(account, 4, fourPity, wishNumber, timeUTC, out wish);
			}
			else
			{
				WishInternalThreeStar(threePity, wishNumber, timeUTC, out wish);
			}
		}

		if (wish.Item is null)
			throw new NotImplementedException();

		account.AddWish(this, wish);
		return wish;
	}

	private BannerEntry PickRandom(uint rarity, bool isGuaranteed, out bool win5050)
	{
		bool is5050 = rnd.NextDouble() <= (FiftyFiftyChance / 100.0);
		BannerEntry[] pool = (rarity <= 3 || rarity == 6
			? entries.Where(w => w.Item.Rarity == rarity) 
			: entries.Where(w => w.Item.Rarity == rarity).Where(w => !(isGuaranteed || is5050) || w.Guaranteed )
			).ToArray();

		Debug.Assert(pool.Length != 0);

		BannerEntry result = pool[rnd.Next(0, pool.Length)];

		win5050 = is5050 || result.Guaranteed;
		return result;
	}

	private void WishInternalSixStar(uint pity, uint wishNumber, DateTime timeUTC, out WishInfo wish)
	{
		BannerEntry entry = PickRandom(6, false, out _);
		wish = new(timeUTC, entry.Item, pity, wishNumber, false, false);
	}

	private void WishInternalThreeStar(uint pity, uint wishNumber, DateTime timeUTC, out WishInfo wish)
	{
		BannerEntry entry = PickRandom(3, false, out _);
		wish = new(timeUTC, entry.Item, pity, wishNumber, false, false);
	}

	private void WishInternal(Account account, uint rarity, uint pity, uint wishNumber, DateTime timeUTC, out WishInfo wish)
	{
		WishInfo? lastWish = account.GetLastWish(this, rarity);

		bool isNextGuaranteed = lastWish?.IsLost5050 ?? false;

		BannerEntry entry = PickRandom(rarity, isNextGuaranteed, out bool win5050);
		wish = new(timeUTC, entry.Item, pity, wishNumber, isNextGuaranteed, win5050);
	}
}