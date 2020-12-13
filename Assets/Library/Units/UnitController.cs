using System.Collections;
using System.Collections.Generic;
using Library.Combat;
using Library.Units;
using UnityEngine;
using UnityEngine.AI;


public class UnitController : MonoBehaviour, IUnitController, ITimed
{
    public Unit unit;
    [HideInInspector]
    public bool playerUnit;

    //nav agent and related fields
    protected NavMeshAgent agent;
    [Header("Navigation")]
    [SerializeField]
    protected float slowedSpeed = 5;
    

    Broker broker;
    ICombat combat;
    //sprite above the unit to dictate status
    SpriteRenderer flag;
    
    private void Awake()
    {
        playerUnit = CompareTag("player_unit");
        
        agent = GetComponent<NavMeshAgent>();
        broker = Camera.main.gameObject.GetComponent<Broker>();
        flag = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        //TODO implement through an InvokeRepeating() and test it, might be an easy way to claw some frames back
        SamplePosition();

        //add a check for combat to keep unit attacking and moving if needed
    }

    /*====================================
     *     COMBAT
     ===================================*/
    public void Attack(UnitController target)
    {
        combat = new Combat(broker, this, target);
    }

    public void Flag(bool flag)
    {
        this.flag.enabled = flag;
    }

    public bool InCombat()
    {
        return combat != null;
    }

    /*====================================
     *     NAVIGATION
     ===================================*/
    public void MoveTo(Vector3 target)
    {
        StartCoroutine(Move(target));
    }

    IEnumerator Move(Vector3 target)
    {
        agent.SetDestination(target);
        while (agent.remainingDistance > broker.stoppingDistance)
        {
            yield return null;
        }
        agent.SetDestination(agent.transform.position);
    }

    //position sampling for slow speeds
    protected void SamplePosition()
    {
        NavMeshHit navMeshHit;            

        float speedToSet = NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8) ?
            slowedSpeed :
            unit.speed;

        agent.speed = speedToSet;
    }   

    /*====================================
     *     TIME
     ===================================*/
    public State Record()
    {
        return new UnitState(
            gameObject,
            transform.position,
            transform.rotation,
            unit.health,
            unit.statuses,
            !(combat == null)
            );
    }
    
    /*====================================
     *     UTILITY
     ===================================*/
    public Unit GetUnit()
    {
        return unit;
    }

    public void LoadAs(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
