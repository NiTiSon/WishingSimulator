namespace WishingSimulator;

public readonly record struct BannerEntry(Item Item, bool Guaranteed)
{
	public static implicit operator BannerEntry(Item Item)
		=> new(Item, false);

	public override int GetHashCode()
	{
		return Item.GetHashCode();
	}

	public override string ToString()
		=> $"{{{Item.Name,-32} {Item.Rarity}* G:{Guaranteed}}}";
}