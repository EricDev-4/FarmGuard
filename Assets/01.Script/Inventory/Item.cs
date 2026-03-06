using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Item
{
  public enum ItemType
    {
        Hoe,
        Axe,
        PickAxe,
        Basket,
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch(itemType)
        {
            default:
            case ItemType.Axe : return ItemAssets.Instance.AxeSprite;
            case ItemType.PickAxe : return ItemAssets.Instance.PickAxeSprite;
            case ItemType.Hoe : return ItemAssets.Instance.HoeSprite;
            case ItemType.Basket : return ItemAssets.Instance.BasketSprite;
        }
    }

    public bool IsEquippable()
    {
        switch(itemType)
        {
            case ItemType.Hoe:
            case ItemType.Axe:
            case ItemType.PickAxe:
                return true;
            default:
                return false;
        }
    }
}
