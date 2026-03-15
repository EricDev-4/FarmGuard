using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public BaseItemSO itemSO;

    public void Pickup()
    {
        InventoryManager.instance.AddItem(itemSO);
        Destroy(gameObject);
    }
}

// WorldItem 에 RockSO 만들어서 넣기