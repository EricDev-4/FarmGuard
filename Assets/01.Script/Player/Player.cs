using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder; // 손 위치 빈 오브젝트

    [SerializeField] private int currentSlot;
    private GameObject equippedWeapon;
    private InventoryItem inventoryItem;

    [SerializeField] Collider interactionCol;
    private int lastSelectedSlot = -1;


    SoilTile soilTile;
    private bool clickFlag = false;
    
    private void Awake()
    {
        currentSlot = 0;
    }

    void Start()
    {
        soilTile = GetComponent<SoilTile>();
    }
    private void Update()
    {
        EquipSelectedWeapon();

        HandlePlowInput();
    }

    private void HandlePlowInput()
    {
        bool isHoe = inventoryItem != null && inventoryItem.item.actionType == ItemSO.ActionType.Hoe;

        if (Input.GetMouseButtonDown(0) && !clickFlag)
        {
            if (isHoe)
            {
                clickFlag = true;
                soilTile.plowingSoil();
            }
            else
            {
                Debug.Log("땅을 파려면 Hoe를 사용하세요!");
            }
        }
        else if (Input.GetMouseButtonUp(0) && clickFlag && isHoe)
        {
            clickFlag = false;
        }
    }

    private void EquipSelectedWeapon()
    {
        int currentSlot = InventoryManager.instance.selectedSlot;

        if (currentSlot == lastSelectedSlot) return;

        lastSelectedSlot = currentSlot;

        foreach (Transform child in weaponHolder)
            Destroy(child.gameObject);

        if (currentSlot < 0) return;

        inventoryItem = InventoryManager.instance.inventorySlots[currentSlot]
            .GetComponentInChildren<InventoryItem>();

        if (inventoryItem != null && inventoryItem.item.ItemPrefab != null)
        {
            Instantiate(inventoryItem.item.ItemPrefab, weaponHolder);
        }
    }

}
