using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHandler_Proto : MonoBehaviour
{
    float health;
    float attack;
    float attackSpeed;
    bool isInCombat;
    GameObject[] enemyArray;

    void Start()
    {
        health = 100f;
        attack = 10f;
        attackSpeed = 5f;
        enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            isInCombat = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(isInCombat == true)
        {

        }
        
    }
}
