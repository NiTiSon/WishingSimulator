using static WishingSimulator.WeaponKind;

namespace WishingSimulator;

public static class Items
{
	public static WeaponItem
		// 5-star weapons
		RoyalBreeze,
		RoyalJudgment,
		RoyalForesight,

		// 4-star
		FrostSword,
		BlacksmithBend,

		// 3-star weapon
		BluntSword,
		SoakedBook,
		WeakBow,
		BeginnersBow
		;

	static Items()
	{
		RoyalBreeze = new(5, "Королевский взмах", Sword);
		RoyalJudgment = new(5, "Королевский ум", Catalyst);
		RoyalForesight = new(5, "Королевский взор", Bow);

		BlacksmithBend = new(4, "Кузнечный сгиб", Bow);
		FrostSword = new(4, "Клинок мороза", Sword);

		BluntSword = new(3, "Тупой меч", Sword);
		SoakedBook = new(3, "Промокшая книга", Catalyst);
		WeakBow = new(3, "Лучок", Bow);
		BeginnersBow = new(3, "Лук новичка", Bow);
	}
}