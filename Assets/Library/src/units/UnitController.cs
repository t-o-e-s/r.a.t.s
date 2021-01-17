using System.Collections;
using System.Collections.Generic;
using Library.src.combat;
using Library.src.time;
using Library.src.time.records;
using Library.src.util;
using UnityEngine;
using UnityEngine.AI;

namespace Library.src.units
{
    public class UnitController : TimeSensitive, IUnitController
    {
        [HideInInspector]
        public Unit unit;
        [HideInInspector]
        public bool playerUnit;

        //nav agent and related fields
        NavMeshAgent agent;
        [Header("Navigation")]
        [SerializeField]
        protected float slowedSpeed = 5;

        Animator anim;
        Broker broker;
        //sprite above the unit to dictate status
        SpriteRenderer flag; 
        
        //combat related fields
        Brawl brawl = null;
        Unit targetUnit;

        IOHandler io;

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
            StartCoroutine(Move(target.transform.position, true));
        }

        public void DealDamage()
        {
            //TODO create a proper damage calculation
            var damage = playerUnit ? 10f : 5f;
            targetUnit.health -= damage;
            if (targetUnit.health <= 0f)
            {
                targetUnit.controller.Die();
                if (brawl) brawl.RemoveUnit(targetUnit.controller);
                targetUnit = null;
            }

            //TODO give damage to enumerator
            //TODO deal it to enemy
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
            StartCoroutine(Move(target, false));
        }

        IEnumerator Move(Vector3 target, bool toAttack)
        {
            var stoppingDistance = toAttack ? unit.weapon.range : EnvironmentUtil.STOPPING_DISTANCE;
            
            agent.SetDestination(target);
            anim.SetBool("move", true);
            var lastRot = transform.rotation.y;
            
            while (Vector3.Distance(target, transform.position) > stoppingDistance)
            {
                var rot = transform.rotation.y - lastRot;
                anim.SetFloat("turning", rot);
                lastRot = transform.rotation.y;
                yield return null;
            }

            anim.SetBool("move", false);
            agent.SetDestination(agent.transform.position);

            if (toAttack && !InCombat()) InitiateBrawl();
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
            StartCoroutine(Move(target, false));
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
        
        /*====================================
        *     INFO
        ===================================*/
        public override void Record()
        {
            previousRecords.Add(RecordUnit(unit));
        }

        public override void PlayRecord(Record record)
        {
            if (rewindRoutine == null)
            {
                StopAllCoroutines();
                rewindRoutine = StartCoroutine(PlayRecordRoutine((UnitRecord) record));
            }
        }

        IEnumerator PlayRecordRoutine(UnitRecord record)
        {
            agent.SetDestination(record.position);
            do
            {
                while (agent.remainingDistance > EnvironmentUtil.STOPPING_DISTANCE)
                {
                    yield return null;
                }

                agent.SetDestination(((UnitRecord) PreviousRecord()).position);
            } while (IsRewinding());

            rewindRoutine = null;
        }
    }
}
