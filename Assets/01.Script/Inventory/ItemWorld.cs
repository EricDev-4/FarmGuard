using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    // public static ItemWorld SpawnItemWorld(Vector3 position ,Item item)
    // {
    //     Debug.Log(item.itemType);
    //     Transform transform = Instantiate(ItemAssets.Instance.GetWeaponPrefab(item.itemType).transform , position, Quaternion.identity);

    //     ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
    //     itemWorld.SetItem(item);

    //     return itemWorld;
    // }

    public Rigidbody rigid;
    [SerializeField] private Item.ItemType itemType;
    [SerializeField] private int amount = 1;
    private Item item;
    private SpriteRenderer spriteRenderer;
    private bool isPickedUp = false;
    private bool canPickUp = false;

    void Awake()
    {
        // 필요시 아이템 코드 생성
        SetItem(new Item{ itemType = itemType, amount = amount});
        rigid = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

        Invoke("EnablePickUp", 0.5f);
    }

    private void EnablePickUp()
    {
        canPickUp = true;
    }

    public void SetItem(Item item)
    {
        this.item = item;
    }

    public Item GetItem()
    {
        return item;
    }

    public bool TryPickUp()
    {
        if (!canPickUp) return false;
        if (isPickedUp) return false;
        isPickedUp = true;
        return true;
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }

    public static ItemWorld DropItem(Vector3 position ,Item.ItemType itemType)
    {
        Debug.Log(itemType);
        Transform transform = Instantiate(ItemAssets.Instance.GetWorldWeaponPrefab(itemType).transform , position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        // itemWorld.SetItem(item);

        return itemWorld;
    }
}
