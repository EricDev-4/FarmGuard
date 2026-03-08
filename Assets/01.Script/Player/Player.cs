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
    [SerializeField] Transform[] crops;
    [SerializeField] int cropsIndex = 1;



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

        if(Input.GetMouseButtonDown(1))
            PlantCrop();
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

            BaseItemSO baseItemSO = inventoryItem.item;
            if(baseItemSO != null)
            {
                // if(baseItemSO is ItemSO itemSO)
                // {
                //     Instantiate(itemSO.ItemPrefab, weaponHolder);
                // }
                // else if(baseItemSO is CropsSO cropsSO)
                // {
                //     Instantiate(cropsSO.CropsPrefab, weaponHolder);
                // }
                // else if(baseItemSO is SeedBoxSO seedBoxSO)
                // {
                //     Instantiate(seedBoxSO.Seedbox, weaponHolder);
                // }
                Instantiate(baseItemSO.holdPrefab, weaponHolder);
            }
        }

    private void HandlePlowInput()
    {
        ItemSO itemSO = inventoryItem?.item as ItemSO;
        bool isHoe = itemSO != null && itemSO.actionType == ItemSO.ActionType.Hoe;

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
    private void PlantCrop()
    {
       
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hitInfo , rayDistance);
        if(hitInfo.transform == null) return;
        SeedBoxSO seedBoxSO = inventoryItem.item as SeedBoxSO;
        if(seedBoxSO != null && hitInfo.transform.tag == "DrySoil" && hitInfo.transform.childCount == 0)
        {
            crops = seedBoxSO.seedPrefab.GetComponentsInChildren<Transform>(true);
            foreach(Transform transform in crops)
            {
                transform.gameObject.SetActive(false);
            }
            crops[cropsIndex].gameObject.SetActive(true);
            Transform temp = Instantiate(crops[0] ,hitInfo.transform);
            temp.gameObject.SetActive(true);
        }  
    }

    private void WaterCrop()
    {
        if(hitInfo.transform != null && hitInfo.transform.tag == "DrySoil")
        {
            
        }
    }
}


