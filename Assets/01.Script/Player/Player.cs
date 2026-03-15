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


    
    // TODO: 나중에 Crops 스크립트로 옮기기
    [Header("Crops")]
    RaycastHit hitInfo;
    Ray ray;
    [SerializeField] float rayDistance;
    [SerializeField] float pickupRange = 2f;
    [SerializeField] Transform[] crops;
    [SerializeField] int cropsIndex = 1;



    private GameObject equippedWeapon;
    [SerializeField] private InventoryItem inventoryItem;

    [SerializeField] Collider interactionCol;
    private int lastSelectedSlot = -1;
    private BaseItemSO lastEquippedItem = null;


    SoilTile soilTile;
    
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
        Ray();
        HandleInput();
        Debug.DrawRay(transform.position , Vector3.forward , Color.blue, rayDistance );
        if (Input.GetKeyDown(KeyCode.E))
            TryPickupItem();

        if (Input.GetMouseButtonDown(1))
        {
            PlantCrop();
            WaterCrop();
        }
    }

    private void Ray()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo, rayDistance);
    }

    private void EquipSelectedWeapon()
    {
        int currentSlot = InventoryManager.instance.selectedSlot;

        InventoryItem slotItem = currentSlot >= 0
            ? InventoryManager.instance.inventorySlots[currentSlot].GetComponentInChildren<InventoryItem>()
            : null;

        BaseItemSO currentItem = slotItem?.item;

        if (currentSlot == lastSelectedSlot && currentItem == lastEquippedItem) return;

        lastSelectedSlot = currentSlot;
        lastEquippedItem = currentItem;

        foreach (Transform child in weaponHolder)
            Destroy(child.gameObject);

        inventoryItem = slotItem;

        if (currentItem != null)
            Instantiate(currentItem.holdPrefab, weaponHolder);
    }


    private void HandleInput()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        ItemSO itemSO = inventoryItem?.item as ItemSO;
        if(itemSO == null ) return;
        switch(itemSO.actionType)
        {
            case ItemSO.ActionType.Hoe :
                soilTile.plowingSoil();
            break;
            case ItemSO.ActionType.Pickaxe:
                if(hitInfo.transform == null) return;
                if(hitInfo.transform.tag == "Rock")
                {
                    Rock rock = hitInfo.transform.GetComponent<Rock>();
                    rock.currentHealth -= itemSO.Damage;
                    Debug.Log("돌 체력 :" + rock.currentHealth);
                }
            break;
            case ItemSO.ActionType.Axe:
                if (hitInfo.transform == null) return;
                if (hitInfo.transform.tag == "Tree")
                {
                    Wood wood = hitInfo.transform.GetComponent<Wood>();
                    if (wood != null)
                    {
                        wood.Damaged(itemSO.Damage);
                    }
                }
                    break;
        }
    }
    private void TryPickupItem()
    {
        if (hitInfo.transform == null) return;
        if (Vector3.Distance(transform.position, hitInfo.point) > pickupRange) return;
        WorldItem worldItem = hitInfo.transform.GetComponent<WorldItem>();
        worldItem?.Pickup();
    }

    private void PlantCrop()
    {
        if(inventoryItem == null || inventoryItem.item == null) return;

        SeedBoxSO seedBoxSO = inventoryItem.item as SeedBoxSO;
        if(seedBoxSO == null) return;

        
        if(hitInfo.transform == null) return;
        if(hitInfo.transform.tag == "DrySoil" && hitInfo.transform.childCount == 0)
        {
            crops = seedBoxSO.seedPrefab.GetComponentsInChildren<Transform>(true);
            foreach(Transform transform in crops)
            {
                transform.gameObject.SetActive(false);
            }
            crops[cropsIndex].gameObject.SetActive(true);
            Transform temp = Instantiate(crops[0] ,hitInfo.transform);
            temp.gameObject.SetActive(true);
            crops = null;
        }  
    }

    private void WaterCrop()
    {
        ItemSO itemSO = inventoryItem?.item as ItemSO;
        bool isWateringCan = itemSO != null && itemSO.actionType == ItemSO.ActionType.WateringCan;

        if(!isWateringCan) return;

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hitInfo, rayDistance);

        if(hitInfo.transform != null && hitInfo.transform.CompareTag("DrySoil"))
        {
            soilTile.wateringSoil(hitInfo.transform);
        }
    }
}


