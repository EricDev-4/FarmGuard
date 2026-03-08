using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BaseItemSO item;
    public int count = 1;
     public Transform originalParent;
    
    public Image image;
    public TextMeshProUGUI countText;

    public void InitaliseItem(BaseItemSO newItem)
    {  
        item = newItem;
        image.sprite = newItem.itemImage;
        // RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(originalParent);
    }
}
