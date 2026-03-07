using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{

    public Image image;
    public Color selectedColor , notSelectedColor;

    private void Awake()
    {
        Deslect();
    }

    public void Select()
    {
        Debug.Log("Selceasd");
        image.color =  selectedColor;
    }

    public void Deslect()
    {
        image.color = notSelectedColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            inventoryItem.originalParent = transform;
        }
    }
}
