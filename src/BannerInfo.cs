using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace WishingSimulator;

public sealed class BannerInfo
{
	private readonly List<WishInfo> wishes;

	public BannerInfo()
	{
		wishes = [];
	}

	public IEnumerable<WishInfo> Wishes
		=> wishes.Reverse<WishInfo>().ToImmutableArray();

	public uint WishesCount
		=> (uint)wishes.Count;

	public void Add(WishInfo wish)
		=> wishes.Add(wish);
}