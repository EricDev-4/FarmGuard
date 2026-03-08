using UnityEngine;

public abstract class BaseItemSO : ScriptableObject
{
    [Header("UI")]
    public Sprite itemImage;
    public bool stackable = false;

    [Header("Hold")]
    public GameObject holdPrefab;
}
