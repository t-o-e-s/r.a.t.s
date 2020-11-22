using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour, IUnit, ITimed
{    

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
    public CombatResolver combat = null;
    //unit that this one is targeting
    [HideInInspector]
    public UnitController combatTarget;


    //nav agent and related fields
    protected NavMeshAgent agent;
    [Header("Navigation")]
    [SerializeField]
    public float speed = 10;
    [SerializeField]
    protected float slowedSpeed = 5;
    [SerializeField]
    protected float combatStoppingDistance = 2f;

    //sprite above the unit to dictate status
    SpriteRenderer flag;


    Status unitStatus;

    //METHODS ==========================================================================================
    // IUnit methods (abstract)  
    public abstract void SetStats();


    //implementations of monobehaviours 
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

        //add a check for combat to keep unit attacking and moving if needed
    }

    //implementations of IUnit
    public virtual void Attack(UnitController target)
    {
        Debug.Log(name + ": is attacking -> " + target.name);

        if (target.InCombat())
        {
            combat = target.combat;
            combat.Add(this);
        }
        else
        {
            combat = new CombatResolver();
            combat.Init(this, target);
        }

        combatTarget = target;
    }

    public void Flag(bool flag)
    {
        this.flag.enabled = flag;
    }

    public void Move(Vector3 target)
    {
        agent.SetDestination(target);
    }

    //public methods for updating unit attributes
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
    //this is defaulted to melee units, override in ranged unit controllers for more succint behaviours
    protected IEnumerator MoveToAttack(UnitController target)
    {
        Debug.Log("agent at " + agent.transform.position.ToString() +  " will move to " + target.transform.position.ToString());
        agent.SetDestination(target.gameObject.transform.position);
        Debug.Log(agent.destination);
       
        Debug.Log(agent.remainingDistance);
    
        Debug.Log(agent.remainingDistance > combatStoppingDistance);

        while (agent.remainingDistance > combatStoppingDistance)
        {
            Debug.Log("moving");
            yield return null;
        }

        agent.isStopped = true;

        //if the player fails to arrive pull out of combat
        if (!combat.Arrived(this, combatTarget))
        {
            combatTarget = null;
        }
    }

    //position sampling for slow speeds
    protected void SamplePosition()
    {
        NavMeshHit navMeshHit;            

        float speedToSet = NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8) ?
            slowedSpeed :
            speed;

        agent.speed = speedToSet;
    }   

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
}
