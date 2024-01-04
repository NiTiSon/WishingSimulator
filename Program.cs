namespace WishingSimulator;

internal class Program
{
	static void Main(string[] args)
	{
		Account user = new();

		Banner characterBanner = new(BannerKind.Character, true,
			// 5-star guaranteed characters
			Characters.Ewalt.Guaranteed,
			Characters.Dekoto.Guaranteed,
			
			// 4-star guaranteed weapon
			Items.OmniBow.Guaranteed,
			
			// 4-star guaranteed characters
			Characters.Roshi.Guaranteed,
			Characters.Sorra.Guaranteed
			);

		Console.WriteLine("│ Pity │ Roll │ Star │ Name");
		Console.WriteLine("├──────┼──────┼──────┼─────────────────────────");

		for (int i = 0; i < 90; i++)
		{
			WishInfo wish = characterBanner.Wish(user);
			
			if (wish.Item.Rarity == 5)
				Console.ForegroundColor = ConsoleColor.Yellow;
			else if (wish.Item.Rarity == 4)
				Console.ForegroundColor = ConsoleColor.Magenta;

			char l = ' ';
			if (wish.Item.Rarity > 3)
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
	}
}