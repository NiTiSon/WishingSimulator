namespace WishingSimulator;

public class Item
{
	internal static List<Item> allItems = new(32);

	public string Name { get; set; }
	public uint Rarity { get; private set; }

	public Item(uint rarity, string name)
	{
		Rarity = rarity;
		Name = name;

		allItems.Add(this);
	}

	public BannerEntry Guaranteed
		=> new(this, true);

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Rarity);
	}
}