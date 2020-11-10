using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour, IUnit
{
    //will be used for the individual units under the controller
    protected Dictionary<NavMeshAgent, Vector3> entities = new Dictionary<NavMeshAgent, Vector3>();

    //combat this unit is in
    public CombatResolver combat = null;

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


        //
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (!t.CompareTag("entity")) continue;

            Vector3 offset = transform.position - t.position;

            NavMeshAgent childAgent;
            if (t.gameObject.TryGetComponent(out childAgent))
            {
                entities.Add(childAgent, offset);
            }
        }             
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

        foreach (KeyValuePair<NavMeshAgent, Vector3> pair in entities)
        {
            // needs elaboration, but works
            pair.Key.SetDestination(target + pair.Value);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage; 
    }

    public void Heal(float heal)
    {
        health += heal;
    }

    public bool InCombat()
    {
        return combat != null;
    }

    //new implementations
    protected void SamplePosition()
    {
        NavMeshHit navMeshHit;            

        float speedToSet = NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8) ?
            slowedSpeed :
            speed;

        agent.speed = speedToSet;

        foreach (KeyValuePair<NavMeshAgent, Vector3> pair in entities)
        {
            pair.Key.speed = speedToSet;

        }
    }   
}
