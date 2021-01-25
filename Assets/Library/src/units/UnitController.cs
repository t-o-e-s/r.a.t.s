using System.Collections;
using System.Collections.Generic;
using Library.src.animation;
using Library.src.animation.impl;
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
        //unit
        [HideInInspector]
        public Unit unit;
        [HideInInspector]
        public bool playerUnit;

        //navigation
        NavMeshAgent agent;
        [Header("Navigation")]
        [SerializeField]
        protected float slowedSpeed = 5;

        //sprite above the unit to dictate status
        SpriteRenderer flag; 
        
        //combat related fields
        Brawl brawl;
        Unit targetUnit;
        [SerializeField] float defence;
        bool isAttacker;

        //referenced from camera
        Broker broker;
        IOHandler io;


    
        void Awake()
        {
            playerUnit = CompareTag("player_unit");
        
            agent = GetComponent<NavMeshAgent>();
            anim = new RatAnimationController(GetComponent<Animator>());
            flag = GetComponentInChildren<SpriteRenderer>();

            targetUnit = null;

            var cam = Camera.main;
            broker = cam.GetComponent<Broker>();
            io = cam.GetComponent<IOHandler>();

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
            isAttacker = true;
        }     
        
        public void DealDamage()
        {
            var x = isAttacker ? 1.0f : 0.5f;

            var damageDone = unit.weapon.damage * (unit.weapon.speed / 10f) - targetUnit.controller.defence * x;
            targetUnit.health -= damageDone;

            if (targetUnit.health <= 0f)
            {
                targetUnit.controller.Die();
                if (brawl) brawl.RemoveUnit(targetUnit.controller);
                targetUnit = null;
                isAttacker = false;
                anim.SetBrawling(false);
            }
        }

        public void FightAnimation()
        {                        
            anim.SetSlash();
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
            anim.SetMoving(true);
            var lastRot = transform.rotation.y;
            
            while (Vector3.Distance(target, transform.position) > stoppingDistance)
            {
                anim.SetTurning(transform.rotation.y - lastRot);
                lastRot = transform.rotation.y;
                yield return null;
            }

            anim.SetMoving(false);
            agent.SetDestination(agent.transform.position);

            if (toAttack && !InCombat()) InitiateBrawl();
        }

        /*====================================
        *     Loot
        ===================================*/

        public void FetchLoot(Vector3 target)
        {
            StartCoroutine(Move(target, false));
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("loot"))
            {
                
                Destroy(other.gameObject);
                anim.SetMoving(false);
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
