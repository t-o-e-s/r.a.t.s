using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EntityHandler : MonoBehaviour, IUnit
{
    //nav agent and related fields
    private NavMeshAgent agent;
    private float speed;
    private float slowedSpeed;
    Vector3 unit;
    public float stopDistance;
    //sprite above the unit to dictate status
    SpriteRenderer flag;

    GameObject unitLead;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        flag = GetComponentInChildren<SpriteRenderer>();

        unitLead = GameObject.Find("unit_b");

        stopDistance = 2f;
    }

    private void Update()
    {
        unit = unitLead.transform.position;
        agent.SetDestination(unit);
        agent.stoppingDistance = stopDistance;
    }

    public void Attack(GameObject target)
    {
        
    }

    public void Flag(bool flag)
    {
        this.flag.enabled = flag;
    }

    public void Move(Vector3 unit)
    {
       
    } 
}
