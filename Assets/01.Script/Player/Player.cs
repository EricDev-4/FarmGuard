using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private UI_Inventory uIInventory;
    [SerializeField] private Transform weaponHolder; // 손 위치 빈 오브젝트

    [SerializeField] private int currentSlot;

    [SerializeField] List<EquipWeapon> HavesWeapons = new List<EquipWeapon>();
    private GameObject equippedWeapon;

    [SerializeField] Collider interactionCol;

    void Awake()
    {
        currentSlot = 0;
        inventory = new Inventory();
        uIInventory.SetInventory(inventory);
        HavesWeapons = weaponHolder.GetComponentsInChildren<EquipWeapon>().ToList();
    }

    void Update()
    {
        SloatChange();

        if (Input.GetKeyDown(KeyCode.Tab))
            uIInventory.ToggleInventory();

        if(Input.GetKeyDown(KeyCode.Q))
            DropItem();
    }
    // 아이테 먹었을 시
    private void OnTriggerEnter(Collider collider)
    {
        ItemWorld itemWorld = collider.GetComponent<ItemWorld>();
        if(itemWorld != null && itemWorld.TryPickUp())
        {
            Item item = itemWorld.GetItem();
            inventory.AddItem(item);
            itemWorld.DestorySelf();

            if(item.IsEquippable())
                EquipWeapon(item);
        }
    }

    private void EquipWeapon(Item item)
    {
        GameObject prefab = ItemAssets.Instance.GetWeaponPrefab(item.itemType);
        if(prefab == null ) return;

        equippedWeapon = Instantiate(prefab, weaponHolder);
        equippedWeapon.transform.localPosition = Vector3.zero;
        equippedWeapon.transform.localRotation = Quaternion.identity;

        // HavesWeapons.Add(equippedWeapon.GetComponent<EquipWeapon>());
        EquipWeapon ew = equippedWeapon.GetComponent<EquipWeapon>();
        ew.item = item;
        HavesWeapons.Add(ew);

        UpdateActiveWeapon();
    }

    private void SloatChange()
    {
        int prevSlot = currentSlot;

        if (Input.GetKeyDown(KeyCode.Alpha1)) currentSlot = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) currentSlot = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) currentSlot = 2;

        if (prevSlot != currentSlot)
            UpdateActiveWeapon();
    }

    private void UpdateActiveWeapon()
    {
        if (HavesWeapons.Count == 0) return;
        if (currentSlot >= HavesWeapons.Count) return; // 범위 초과 방지

        foreach (EquipWeapon ew in HavesWeapons)
            ew.gameObject.SetActive(false);

        HavesWeapons[currentSlot].gameObject.SetActive(true);
    }

    private void DropItem()
    {
        if(HavesWeapons.Count == 0 || currentSlot >= HavesWeapons.Count) return;

        EquipWeapon weapon = HavesWeapons[currentSlot];

        inventory.RemoveItem(weapon.item);

        HavesWeapons.RemoveAt(currentSlot);

        Destroy(weapon.gameObject);

        ItemWorld itemWorld = ItemWorld.DropItem(transform.position, weapon.item.itemType);

        if(itemWorld != null)
            itemWorld.rigid.AddForce((interactionCol.transform.forward + interactionCol.transform.up) * 3f , ForceMode.Impulse );

        
        

        if(currentSlot >= HavesWeapons.Count)
            currentSlot = HavesWeapons.Count -1;

        UpdateActiveWeapon();
    }
}
