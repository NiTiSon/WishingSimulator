﻿namespace WishingSimulator;

public sealed class CharacterItem : Item
{
	public CharacterItem(uint rarity, string name) : base(rarity, name)
	{

	}

	public override Instance CreateInstance()
	{
		return new Instance(this);
	}

	public sealed new class Instance : Item.Instance
	{
		public uint Level { get; private set; }

		internal Instance(Item instanceOf) : base(instanceOf)
		{
			Level = 1;
		}
	}
}