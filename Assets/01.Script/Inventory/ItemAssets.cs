using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set;}

    private void Awake()
    {
        Instance = this;
    }

    public GameObject w_pfHoeWeapon;
    public GameObject w_pfAxeWeapon;
    public GameObject w_pfPickAxeWeapon;

    public Sprite HoeSprite;
    public Sprite AxeSprite;
    public Sprite PickAxeSprite;
    public Sprite BasketSprite;

    public GameObject pfHoeWeapon;
    public GameObject pfAxeWeapon;
    public GameObject pfPickAxeWeapon;

    public GameObject GetWeaponPrefab(Item.ItemType itemType)
    {
        switch(itemType)
        {
            case Item.ItemType.Hoe:     return pfHoeWeapon;
            case Item.ItemType.Axe:     return pfAxeWeapon;
            case Item.ItemType.PickAxe: return pfPickAxeWeapon;
            default: return null;
        }
    }

        public GameObject GetWorldWeaponPrefab(Item.ItemType itemType)
    {
        switch(itemType)
        {
            case Item.ItemType.Hoe:     return w_pfHoeWeapon;
            case Item.ItemType.Axe:     return w_pfAxeWeapon;
            case Item.ItemType.PickAxe: return w_pfPickAxeWeapon;
            default: return null;
        }
    }
}
