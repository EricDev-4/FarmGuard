using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; 


    public List<BaseItemSO> itemList = new List<BaseItemSO>();
    public int maxStackedItems = 10;
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    
    [SerializeField] RectTransform inventoryTab;

    public int selectedSlot = -1;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryTab.gameObject.SetActive(!inventoryTab.gameObject.activeSelf);   
        }
        if(Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if(isNumber && number > 0 && number < 8)
                ChangedSelectedSlot(number -1);
        }


        // if(selectedSlot >= 0 && inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>() != null)
        //     Debug.Log(inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>().item);
    }

    void ChangedSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0) // selectedSlot 가 0보다 크면
        {
            inventorySlots[selectedSlot].Deslect(); // 현재 selectedSlot의 inventorySlots를 Deslect 
        }

        inventorySlots[newValue].Select(); // newValue(입력된 값)의 Slots.Select
        selectedSlot = newValue; // selectedSlot 에는 newValue(입력된 값) 대입
    }


    public void AddItem(BaseItemSO item)
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            // slot 내부의 item이 비어있지 않고 , 내부의 item과 AddItem에서 전달받는 item이 같으며 count는 max를 넘지 않고 stack이 가능할 때
            if(itemInSlot != null &&
                itemInSlot.item == item &&
                itemInSlot.count < maxStackedItems &&
                itemInSlot.item.stackable == true
                )
            {
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return;
            }
        }

        for(int i =0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if(itemInSlot == null)
            {
                AddNewItem(item,slot);
                return;
            }
        }
    } 


    void AddNewItem(BaseItemSO item , InventorySlot slot)
    {
        itemList.Add(item);
        GameObject newItemGo = Instantiate(inventoryItemPrefab , slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitaliseItem(item);
    }

    public bool isInventoryOpen()
    {
        return inventoryTab.gameObject.activeSelf;
    }
}
