using System.Collections;
using System.Collections.Generic;
using Library.Combat;
using Library.Units;
using UnityEngine;
using UnityEngine.AI;

public class UnitControllerController : MonoBehaviour, IUnitController, ITimed
{
    public Unit unit; 
    [HideInInspector]
    public bool playerUnit;

    [Header("Unit Attributes")]
    //health, duh
    public float health;
    //attack
    public float attack;
    //defense
    public float defense;

    //array of applied statuses
    protected Status[] statuses;

    //combat this unit is in
    [HideInInspector]
    public ICombat combat;
    //unit that this one is targeting
    [HideInInspector]
    public UnitControllerController combatTarget;


    //nav agent and related fields
    protected NavMeshAgent agent;
    [Header("Navigation")]
    [SerializeField]
    public float speed = 10;
    [SerializeField]
    protected float slowedSpeed = 5;
    [SerializeField]
    protected float stoppingDistance = 0.4f;

    //sprite above the unit to dictate status
    SpriteRenderer flag;


    Status unitStatus;

    //METHODS ==========================================================================================
    // IUnit methods (abstract)  
    public abstract void SetStats();


    /* ========================================
     * MONO
       ======================================*/
    private void Awake()
    {
        //checking to see if this is a player controlled object
        playerUnit = CompareTag("player_unit");

        agent = GetComponent<NavMeshAgent>();

        flag = GetComponentInChildren<SpriteRenderer>();

        //all unit stats can be set within this method
        SetStats();         
    }

    private void Update()
    {
        //TODO implement through an InvokeRepeating() and test it, might be an easy way to claw some frames back
        SamplePosition();
    }
    
    /* ========================================
     * UI
       ======================================*/
    public void Flag(bool flag)
    {
        this.flag.enabled = flag;
    }

    /* ========================================
     * NAVIGATION
       ======================================*/
    public void Move(Vector3 target)
    {
        StartCoroutine(MoveTo(target));
    }

    protected IEnumerator MoveTo(Vector3 target)
    {
        agent.SetDestination(target);
        
        while (agent.remainingDistance > stoppingDistance)
        {
            yield return null;
        }

        agent.SetDestination(agent.transform.position);
    }
    
    protected void SamplePosition()
    {
        NavMeshHit navMeshHit;            

        float speedToSet = NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8) ?
            slowedSpeed :
            speed;

        agent.speed = speedToSet;
    }  
    
    /* ========================================
     * COMBAT
       ======================================*/
    public bool InCombat()
    {
        return combat != null;
    }
    
    public virtual void Attack(UnitControllerController target)
    {
        
    }

    /* ========================================
     * CHRONOLOGICAL FUNCTIONS
       ======================================*/
    //implementation of Record() from ITimed - used for saving position
    public State Record()
    {
        return new UnitState(
            gameObject,
            transform.position,
            transform.rotation,
            health,
            statuses,
            !(combat == null)
            );
    }
    
    /* ========================================
     * UTILITY
       ======================================*/
    public void LoadAs(Unit unit)
    {
        this.unit = unit;
        //TODO implement any neccesary changes
    }
}
