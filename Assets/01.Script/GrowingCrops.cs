using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingCrops : MonoBehaviour
{
    public Transform[] crops;
    public Transform currentSoil;

    public int currentActiveIndex;

    public void ActiveCropIndex()
    {
            for (int i = 0; i < crops.Length; i++)
            {
                if (crops[i].gameObject.activeSelf)
                currentActiveIndex = i;
            }
            // return -1; // 활성화된 crop이 없으면 -1 반환
            currentActiveIndex = -1;
    }

    void Start()
    {
        currentSoil = GetComponentInParent<Transform>();
        crops = GetComponentsInChildren<Transform>(true);
    }

    void Update()
    {
        ActiveCropIndex();
        
    }

    private void GrowingCrop()
    {
        crops[currentActiveIndex].gameObject.SetActive(false);
        currentActiveIndex++;
        crops[currentActiveIndex].gameObject.SetActive(true);
    }
}
