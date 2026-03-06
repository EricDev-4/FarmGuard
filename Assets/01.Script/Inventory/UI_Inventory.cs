using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private Transform itemSlotContatiner;
    [SerializeField]private Transform itemSlotTemplate;

    [SerializeField] private GameObject inventory_UI;

    public void ToggleInventory()
    {
        inventory_UI.SetActive(!inventory_UI.activeSelf);
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }

    // 인벤토리 새로고침
    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContatiner)
        {
            if(child == itemSlotTemplate) continue; // 원본 보존
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;

        foreach(Item item in inventory.GetItemList())
        {
            Debug.Log(inventory.GetItemList());
            // itemSlotTemplate 을 생성해 itemSlotContatiner의 자식으로 넣고 RectTransform을 가져옴
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate , itemSlotContatiner).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("Image").GetComponent<Image>();
            image.sprite = item.GetSprite();
            x++;
            if( x > 4)
            {
                x = 0;
                y++;
            }
        }
    }
}
