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

    void Update()
    {
        // Debug.DrawRay(InteractionCollider.transform.position, Vector3.down, Color.red, 100f);
        if(Input.GetMouseButtonDown(0) && !clickFlag)
        {
            clickFlag = true;
            plowingSoil();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            clickFlag = false;
        }
    }

    // private void plowingSoil()
    // {
    //     if (Physics.Raycast(InteractionCollider.transform.position, Vector3.down, out hitInfo, 100f))
    //     {

    //         if (hitInfo.collider.CompareTag(Tags.TillableSoil))
    //         {
    //             // Debug.Log(hitInfo.point);
    //             Vector3 point = hitInfo.point;
    //             Vector3 snappedPosition = new Vector3(
    //                 Mathf.Round(point.x/ gridSize) * gridSize,
    //                 (Mathf.Round(point.y/ gridSize) * gridSize) + 0.01f,
    //                 Mathf.Round(point.z/ gridSize) * gridSize
    //             );
    //             Vector3 GroundPos = hitInfo.transform.localScale;

    //             Debug.Log(snappedPosition);

    //             Instantiate(prefabs[((int)soil.DrySoil)], snappedPosition, Quaternion.identity);
    //         }
    //     }
    // }.
    private void plowingSoil()
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
