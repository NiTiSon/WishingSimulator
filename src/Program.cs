using System;
using System.Linq;

namespace WishingSimulator;

internal sealed class Program
{
	static void Main(string[] args)
	{
		Account user = new();

		Banner characterBanner = new(BannerKind.Character, true,
			// 5-star guaranteed characters
			Characters.Ewalt.Guaranteed,
			Characters.Dekoto.Guaranteed,
			
			// 4-star guaranteed weapon
			Items.BlacksmithBend.Guaranteed,

			// 4-star guaranteed characters
			Characters.Roshi.Guaranteed,
			Characters.Sorra.Guaranteed
			);

		Console.WriteLine("│ Pity │ Roll │ Star │ Name");
		Console.WriteLine("├──────┼──────┼──────┼─────────────────────────");

		for (int i = 0; i < 90; i++)
		{
			WishInfo wish = characterBanner.Wish(user);

			if (wish.Item.Rarity == 6)
				Console.ForegroundColor = ConsoleColor.Cyan;
			else if (wish.Item.Rarity == 5)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else if (wish.Item.Rarity == 4)
				Console.ForegroundColor = ConsoleColor.Magenta;

			char l = ' ';
			if (wish.Item.Rarity is > 3 and not 6)
			{
				if (wish.IsGuaranteed)
					l = 'G';
				else if (wish.IsWon5050)
					l = 'W';
				else
					l = 'L';
			}

			Console.WriteLine($"│ {wish.Pity,-5}│ {wish.WishNumber,-4} │ {wish.Item.Rarity}* {l} │ {wish.Item.Name}");

			Console.ResetColor();
		}

		Console.WriteLine("└──────┴──────┴──────┴─────────────────────────");
		Console.WriteLine($"6*: {user.Inventory.Count(i => i.Of.Rarity == 6)}");
		Console.WriteLine($"5*: {user.Inventory.Count(i => i.Of.Rarity == 5)}");
		Console.WriteLine($"4*: {user.Inventory.Count(i => i.Of.Rarity == 4)}");
		Console.WriteLine($"3*: {user.Inventory.Count(i => i.Of.Rarity == 3)}");
	}
}