using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour, IUnit
{
    //will be used for the individual units under the controller
    protected GameObject[] entities;

    //nav agent and related fields
    protected NavMeshAgent agent;
    [SerializeField]
    protected float speed = 10;
    [SerializeField]
    protected float slowedSpeed = 5;

    //sprite above the unit to dictate status
    SpriteRenderer flag;

    //health, duh
    [SerializeField]
    float health;




    // IUnit methods (left abstract/ without implementation)
    public abstract void Attack(GameObject target);

    //methods that children need to inherit
    public abstract void SetStats();


    //implementations of monobehaviours 
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        flag = GetComponentInChildren<SpriteRenderer>();

        //all unit stats can be set within this method
        SetStats();
    }

    private void Update()
    {
        SamplePosition();
    }

    //implementations of IUnit
    public void Flag(bool flag)
    {
        this.flag.enabled = flag;
    }

    public void Move(Vector3 target)
    {
        agent.SetDestination(target);
    }

    //new implementations
    protected void SamplePosition()
    {
        NavMeshHit navMeshHit;

        if (NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8))
            agent.speed = slowedSpeed; 
    }
}
