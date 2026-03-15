using UnityEngine;

[CreateAssetMenu(menuName = "SO/Rock")]
public class RockSO : BaseItemSO
{
    [Header("Info")]
    public string rockName;

    [Header("Health")]
    public float maxHealth;

    [Header("Drop")]
    public GameObject rockCellPrefab;
    public float rockCellLifetime = 0.5f;
    public GameObject rockItemPrefab;
}
