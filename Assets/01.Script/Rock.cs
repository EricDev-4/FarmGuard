using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField] RockSO rockSO;
    public float currentHealth;
    bool isBroken;

    void Start()
    {
        currentHealth = rockSO.maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0 && !isBroken)
        {
            BreakRock();
        }
    }

    void BreakRock()
    {
        isBroken = true;
        GameObject cell = Instantiate(rockSO.rockCellPrefab, transform.position, transform.rotation);
        Instantiate(rockSO.rockItemPrefab, transform.position, transform.rotation);
        Destroy(this.gameObject);
        Destroy(cell, rockSO.rockCellLifetime);
    }
}
