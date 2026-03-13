using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoilTile : MonoBehaviour
{

    public enum soil
    {
        DrySoil = 0,
        WetMud,
    }

    [SerializeField] GameObject[] prefabs;

    float gridSize;
    SphereCollider InteractionCollider;
    [SerializeField] Transform fieldSize;
    [SerializeField] float gridCount; 
    RaycastHit hitInfo;

    private bool clickFlag = false;

    void Start()
    {
        InteractionCollider =  GetComponentInChildren<SphereCollider>();
        gridSize = fieldSize.localScale.x / gridCount;
    }
    public void plowingSoil()
{
    if (Physics.Raycast(InteractionCollider.transform.position, Vector3.down, out hitInfo, 100f))
    {
        Debug.Log(fieldSize.localScale.x + "/" + gridCount + " = " + gridSize);
        if (hitInfo.collider.CompareTag(Tags.TillableSoil))
        {
            Vector3 point = hitInfo.point;
            float offset = gridSize / 2f; // 칸 중앙으로 이동

            Vector3 snappedPosition = new Vector3(
                Mathf.Floor(point.x / gridSize) * gridSize + offset,
                point.y + 0.01f,
                Mathf.Floor(point.z / gridSize) * gridSize + offset
            );

            Debug.Log(snappedPosition);
            GameObject obj = Instantiate(prefabs[(int)soil.DrySoil], snappedPosition, Quaternion.identity);

            Renderer rend = obj.GetComponent<Renderer>();
            float scaleRatio = gridSize / rend.bounds.size.x;
            obj.transform.localScale *= scaleRatio;
        }
    }
}
    public void wateringSoil(Transform targetSoil)
    {
        // targetSoil의 위치, 스케일, 자식(작물) 저장
        Vector3 position = targetSoil.position;
        Vector3 scale = targetSoil.localScale;
        List<Transform> children = new List<Transform>();
        foreach(Transform child in targetSoil)
            children.Add(child);

        // 기존 DrySoil 삭제
        Destroy(targetSoil.gameObject);

        // WetMud 프리팹 생성
        GameObject wetSoil = Instantiate(prefabs[(int)soil.WetMud], position, Quaternion.identity);
        wetSoil.transform.localScale = scale;

        // 자식(작물) 다시 붙이기
        foreach(Transform child in children)
            child.SetParent(wetSoil.transform);
    }

    #if UNITY_EDITOR
private void OnDrawGizmos()
{
    if (fieldSize == null) return;

    float gizmoGridSize = fieldSize.localScale.x / gridCount;
    if (gizmoGridSize <= 0) return;

    float fieldWorldSize = fieldSize.localScale.x * 10f;
    float halfSize = fieldWorldSize / 2f;
    float offset = gizmoGridSize / 2f; // 추가
    Vector3 center = fieldSize.position;

    Gizmos.color = new Color(0f, 1f, 0f, 0.4f);

    for (float x = -halfSize; x <= halfSize + 0.001f; x += gizmoGridSize)
    {
        Vector3 from = new Vector3(center.x + x, center.y + 0.02f, center.z - halfSize);
        Vector3 to   = new Vector3(center.x + x, center.y + 0.02f, center.z + halfSize);
        Gizmos.DrawLine(from, to);
    }

    for (float z = -halfSize; z <= halfSize + 0.001f; z += gizmoGridSize)
    {
        Vector3 from = new Vector3(center.x - halfSize, center.y + 0.02f, center.z + z);
        Vector3 to   = new Vector3(center.x + halfSize, center.y + 0.02f, center.z + z);
        Gizmos.DrawLine(from, to);
    }

    // 빨간 점을 칸 중앙으로 이동
    Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
    for (float x = -halfSize; x < halfSize; x += gizmoGridSize)
    {
        for (float z = -halfSize; z < halfSize; z += gizmoGridSize)
        {
            Vector3 snapPoint = new Vector3(center.x + x + offset, center.y + 0.02f, center.z + z + offset);
            Gizmos.DrawSphere(snapPoint, gizmoGridSize * 0.05f);
        }
    }
}
#endif
}
