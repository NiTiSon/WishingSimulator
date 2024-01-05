using System;
using System.Collections.Generic;

namespace WishingSimulator;

public abstract class Item
{
	internal static List<Item> allItems = new(32);

	public string Name { get; set; }
	public uint Rarity { get; private set; }

	public Item(uint rarity, string name)
	{
		Rarity = rarity;
		Name = name;

		if (rarity is > 6 or 0)
			throw new Exception("The range of rarity is between 1 and 6.");

		allItems.Add(this);
	}

	public abstract Instance CreateInstance();

	public BannerEntry Guaranteed
		=> new(this, true);

	public override int GetHashCode()
	{
		return HashCode.Combine(Name, Rarity);
	}

	public abstract class Instance
	{
		protected readonly Item instanceOf;

		protected Instance(Item instanceOf)
		{
			this.instanceOf = instanceOf;
		}
	}
}