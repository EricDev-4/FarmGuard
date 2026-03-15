using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] WoodSO woodSO;
    [SerializeField] float popForce = 5f;
    private float currentHealth;
    bool isBroken;

    private void Start()
    {
        currentHealth = woodSO.maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0 && !isBroken)
        {
            BreakWood();
        }
    }

    public void Damaged(float damage)
    {
        if (isBroken) return;
        currentHealth -= damage;
    }

    void BreakWood()
    {
        isBroken = true;

        GameObject woodItem = Instantiate(woodSO.woodItem, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        Rigidbody rigid = woodItem.GetComponent<Rigidbody>();
        if (rigid != null)
        {
            Vector3 popDir = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
            rigid.AddForce(popDir * popForce, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
