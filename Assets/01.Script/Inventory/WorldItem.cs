using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public BaseItemSO itemSO;

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Player"))
        {
            InventoryManager.instance.AddItem(itemSO);
            Destroy(gameObject);
        }
    }
}
