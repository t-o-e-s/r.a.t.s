using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class UnitController : MonoBehaviour, IUnit, ITimed
{
    //will be used for the individual units under the controller
    protected Dictionary<NavMeshAgent, Vector3> entities = new Dictionary<NavMeshAgent, Vector3>();

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

        unitStatus = Status.normal;

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
        //TODO implement through an InvokeRepeating() and test it, might be an easy way to claw some frames back
        SamplePosition();

        //add a check for combat to keep unit attacking and moving if needed
    }

    //implementations of IUnit
    public virtual void Attack(UnitController target)
    {
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

        foreach (KeyValuePair<NavMeshAgent, Vector3> pair in entities)
        {
            // needs elaboration, but works
            pair.Key.SetDestination(target + pair.Value);
        }
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
        agent.SetDestination(target.gameObject.transform.position);

        while (agent.remainingDistance > combatStoppingDistance)
        {
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

        foreach (KeyValuePair<NavMeshAgent, Vector3> pair in entities)
        {
            pair.Key.speed = speedToSet;

        }
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

    public void StatusEffect()
    {
        switch(unitStatus)
        {
            case Status.normal:
                break;
            case Status.aFlame:
                //TODO call a function for status of aFlame
                break;
            case Status.cold:
                //TODO call a function for status of cold 
                break;
            case Status.spored:
                //TODO call a function for status of spored
                break;
            case Status.wet:
                //TODO call a function for status of wet 
                break;
        }
    
    }

}
