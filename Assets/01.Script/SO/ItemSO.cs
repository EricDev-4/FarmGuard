using UnityEngine;

[CreateAssetMenu(menuName = "SO/Item")]
public class ItemSO : BaseItemSO
{
    [Header("GamePlay")]
    public GameObject WorldPrefab;
    public ItemType itemType;
    public ActionType actionType;

    public enum ItemType
    {
        Tool,
        Useage,
    }
    public enum ActionType
    {
        None , // 기본 값(씨앗 , 소비템 )
        Hoe,        // 괭이  - 땅 갈기
        Axe,        // 도끼  - 나무 베기
        Pickaxe,    // 곡괭이 - 돌/광석 캐기
        WateringCan // 물뿌리개 - 작물에 물주기
    }

}
