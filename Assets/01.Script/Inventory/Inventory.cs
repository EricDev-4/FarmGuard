using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Inventory
{
    public event EventHandler OnItemListChanged;
     private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();
        
        // AddItem(new Item { itemType = Item.ItemType.Hoe , amount = 1});
        // AddItem(new Item { itemType = Item.ItemType.Axe , amount = 1});
        // AddItem(new Item { itemType = Item.ItemType.Basket , amount = 1});

    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
        OnItemListChanged?.Invoke(this , EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.amount > 1)
        {
            item.amount--;
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}

