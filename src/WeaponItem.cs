namespace WishingSimulator;

public class WeaponItem : Item
{
	public WeaponKind Kind { get; }

	public WeaponItem(uint rarity, string name, WeaponKind kind) : base(rarity, name)
	{
		Kind = kind;
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