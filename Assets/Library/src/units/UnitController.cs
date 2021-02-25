using System.Collections;
using Library.src.combat;
using Library.src.time;
using Library.src.time.records;
using Library.src.util;
using UnityEngine;
using UnityEngine.AI;

namespace Library.src.units
{
    public class UnitController : MonoBehaviour, IUnitController, ITimeSensitive
    {
        [HideInInspector]
        public Unit unit;
        [HideInInspector]
        public bool playerUnit;

        //nav agent and related fields
        public NavMeshAgent agent;
        [Header("Navigation")]
        [SerializeField]
        protected float slowedSpeed = 5;
        Coroutine movementRoutine = null;


        Animator anim;
        Broker broker;
        //sprite above the unit to dictate status
        SpriteRenderer flag; 
        
        //combat related fields
        Brawl brawl = null;
        Unit targetUnit;
        [SerializeField] public float defence;
        public bool isAttacker;
        public bool inCombat;
        public bool inVisionCone;
        bool isMoving;
        IOHandler io;
        [SerializeField] [Range(1.0f, 100.0f)]
        public float attackPower;
        [SerializeField] [Range(1, 10)]
        int attackRate;
        [SerializeField]
        [Range(0, 100)]
        public float health;
        
        //time fields
        bool isForwarding = false;
        bool isRewinding = false;

        void Awake()
        {
            playerUnit = CompareTag("player_unit");
        
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            broker = Camera.main.gameObject.GetComponent<Broker>();
            flag = GetComponentInChildren<SpriteRenderer>();

            targetUnit = null;

            io = Camera.main.GetComponent<IOHandler>();
            
            broker.Add(this);
            broker.LoadAs(this);
        }


        /*====================================
        *     COMBAT
        ===================================*/
        public void Attack(UnitController target)
        {
            targetUnit = target.unit;
            Halt();
            movementRoutine = StartCoroutine(MoveRoutine(target.transform, target.transform.position, true));
            this.isAttacker = true;
            inCombat = true;
        }     
        
        public void DealDamage()
        {
            FightAnimation();
            anim.SetBool("inBrawl", true);           
            float x;
            inCombat = true;
            if (isAttacker == true)
            {
                x = 1.0f;
            }
            else
            {
                x = 0.5f;
            }

            float damageDone = 1 * (attackPower * (attackRate / 10f)) - (targetUnit.controller.defence * x);
            targetUnit.health -= damageDone;
            Debug.Log(targetUnit.health);

            if (targetUnit.health <= 0f)
            {
                anim.SetBool("inBrawl", false);
                targetUnit.controller.Die();
                if (brawl) brawl.RemoveUnit(targetUnit.controller);
                targetUnit = null;
                isAttacker = false;
                inCombat = false;
            }

            //TODO give damage to enumerator
            //TODO deal it to enemy
        }

        public void LookAtUnit(Vector3 target)
        {
            Debug.Log("lookAT");
            StartCoroutine(FaceOponent(target));
        }

       IEnumerator FaceOponent(Vector3 target)
        {           
            {
                Quaternion _lookRotation = Quaternion.LookRotation((target - transform.position).normalized);
                
                while (this.inCombat == true)
                {
                    Debug.Log("turn");
                    float turn_speed = 2f;
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turn_speed);
                    anim.SetFloat("turning", 1f);                  
                    yield return null;
                }
            }
        }

        public void FightAnimation()
        {   
            if (inCombat == true)
            anim.SetTrigger("isSlashing");
        }
      
        public void Flag(bool flag)
        {
            this.flag.enabled = flag;
        }

        public void SetTarget(Unit unit)
        {
            targetUnit = unit;
        }

        void InitiateBrawl()
        {
            if (targetUnit.Equals(null)) return;
            
            //setting up brawl object
            brawl = Broker.InitBrawl();
            brawl.AddUnit(this);
            brawl.AddUnit(targetUnit.controller);
        }

        public void Die()
        {
            Destroy(gameObject);
        }

        /*====================================
        *     NAVIGATION
        ===================================*/
        public void MoveTo(Vector3 target)
        {
            Halt();
            movementRoutine = StartCoroutine(MoveRoutine(null, target, false));
        }

        IEnumerator MoveRoutine(Transform target, Vector3 targetPos, bool toAttack)
        {
            var stoppingDistance = toAttack ? unit.weapon.range : EnvironmentUtil.STOPPING_DISTANCE;
            var lastRot = transform.rotation.y;
            targetPos = target != null ? target.position : targetPos;

            agent.SetDestination(targetPos);
            anim.SetBool("move", true);

            while (Vector3.Distance(targetPos, transform.position) > stoppingDistance)
            {
                targetPos = target != null ? target.position : targetPos;
                agent.SetDestination(targetPos);
                var rot = transform.rotation.y - lastRot;
                anim.SetFloat("turning", rot);
                lastRot = transform.rotation.y;

                yield return null;
            }

            anim.SetBool("move", false);
            agent.SetDestination(agent.transform.position);

            if (toAttack && !InCombat())
            {
                InitiateBrawl();
            }
        }        

        public void Halt()
        {
            anim.SetBool("move", false);
            if (movementRoutine != null) StopCoroutine(movementRoutine);
            agent.SetDestination(this.transform.position);
        }

        public void Follow(Transform target, bool toAttack)
        {
           // StartCoroutine(FollowRoutine(target, toAttack));
        }

        //position sampling for slow speeds
        protected void SamplePosition()
        {
            NavMeshHit navMeshHit;            

            var speedToSet = NavMesh.SamplePosition(agent.transform.position, out navMeshHit, 1f, 8) ?
                slowedSpeed :
                unit.speed;

            agent.speed = speedToSet;
        }

        /*====================================
        *     Loot
        ===================================*/

        public void FetchLoot(Vector3 target)
        {
           movementRoutine = StartCoroutine(MoveRoutine(null, target, false));
        }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("loot"))
            {
                
                Destroy(other.gameObject);
                anim.SetBool("move", false);
                agent.SetDestination(agent.transform.position);
                io.TakeLoot();
            }
        }


        /*====================================
        *     TIME
        ===================================*/
        public State Record()
        {
            return null;
        }
    
        /*====================================
        *     UTILITY
        ===================================*/
        public Unit GetUnit()
        {
            return unit;
        }

        public Unit GetTarget()
        {
            return targetUnit;
        }
        
        public void LoadAs(Unit unit)
        {
            this.unit = unit;
        }
        
        /*====================================
        *     INFO
        ===================================*/
        public float GetSpeed()
        {
            return agent.speed;
        }

        bool InCombat()
        {
            return targetUnit != null 
                   && brawl != null;
        }

        public bool SaveRecord()
        {
            throw new System.NotImplementedException();
        }

        public Record GetLastRecord()
        {
            throw new System.NotImplementedException();
        }

        public Record GetNextRecord()
        {
            throw new System.NotImplementedException();
        }

        public void ClearRecords()
        {
            throw new System.NotImplementedException();
        }
    }
}
