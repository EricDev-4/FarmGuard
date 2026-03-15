using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Wood")]
public class WoodSO : BaseItemSO
{
    [Header("Info")]
    public string woodName;

    [Header("Health")]
    public float maxHealth;

    [Header("Drop")]
    public GameObject woodItem;
}
