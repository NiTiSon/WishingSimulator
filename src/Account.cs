﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace WishingSimulator;

public sealed class Account
{
	private readonly Dictionary<BannerKind, BannerInfo> bannersData;
	private readonly List<Item.Instance> inventory;

	public Account()
	{
		inventory = [];
		bannersData = [];
	}

	public ImmutableArray<Item.Instance> Inventory
		=> inventory.ToImmutableArray();

	public void AddWish(Banner from, WishInfo wish)
	{
		if (bannersData.TryGetValue(from.Kind, out var wishes))
		{
			wishes.Add(wish);
			inventory.Add(wish.Item.CreateInstance());
		}
		else
		{
			BannerInfo wishesInfo = new();
			wishesInfo.Add(wish);
			inventory.Add(wish.Item.CreateInstance());

			bannersData[from.Kind] = wishesInfo;
		}
	}

	public uint NextPity(Banner banner, uint rarity)
	{
		if (bannersData.TryGetValue(banner.Kind, out BannerInfo? info))
		{
			uint currentPity = 1;

			foreach (WishInfo wish in info.Wishes)
			{
				if (wish.Item.Rarity != rarity)
					currentPity++;
				else
					break;
			}

			return currentPity;
		}
		else
		{
			return 1;
		}
	}

	public bool IsNextGuaranteed(Banner banner, uint rarity)
	{
		if (bannersData.TryGetValue(banner.Kind, out BannerInfo? info))
		{
			foreach (WishInfo wish in info.Wishes)
			{
				if (wish.Item.Rarity != rarity)
					continue;

				// Last time guaranteed
				if (wish.IsGuaranteed)
					return false;

				// Last time won 50/50
				else if (wish.IsWon5050)
					return false;

				// Last time lost 50/50
				return true;
			}
		}

		// No one
		return false;
	}

	public uint NextWishNumber(Banner banner)
	{
		if (bannersData.TryGetValue(banner.Kind, out BannerInfo? info))
		{
			return info.WishesCount + 1;
		}
		else
		{
			return 1;
		}
	}

	public WishInfo? GetLastWish(Banner banner, uint rarity)
	{
		if (bannersData.TryGetValue(banner.Kind, out BannerInfo? info))
		{
			return info.Wishes.FirstOrDefault(w => w.Item.Rarity == rarity);
		}
		else
		{
			return null;
		}
	}
}