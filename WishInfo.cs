namespace WishingSimulator;

public record class WishInfo(DateTime TimeUTC, Item Item, uint Pity, uint WishNumber, bool IsGuaranteed, bool IsWon5050)
{
	public bool IsLost5050 => !IsWon5050;

	public override string ToString()
	{
		return $"{Item.Rarity}* {Item.Name,-32} {(Item.Rarity <= 3 ? string.Empty : IsGuaranteed ? "Guaranteed" : IsWon5050 ? "Win 50/50" : "Lost 50/50")}";
	}
}