﻿using System;
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
        NavMeshAgent agent;
        [Header("Navigation")]
        [SerializeField]
        protected float slowedSpeed = 5;

        Animator anim;
        Broker broker;
        IOHandler io;
        //sprite above the unit to dictate status
        SpriteRenderer flag;
        //healthbar object above the unit to dictate health, help pls
        GameObject healthBar;
        
        //combat related fields
        Brawl brawl = null;
        Unit targetUnit;
        [SerializeField] [Range(1.0f, 100.0f)]
        public float attackPower;
        [SerializeField] [Range(1, 10)]
        int attackRate;
        [SerializeField] float defence;
        [SerializeField]
        [Range(0, 100)]
        public float health;
        bool isAttacker;
        bool inCombat;
        
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

        void Update()
        {
            //TODO reduce these check to once every x frames to improve performance
            if (isForwarding)
            {
                //TODO move to IOHandler
                GetNextRecord();
            }
            else if (isRewinding)
            {
                //TODO move to IOHandler
                GetLastRecord();
            }
            else
            {
                SaveRecord();
            }
        }

        /*====================================
        *     COMBAT
        ===================================*/
        public void Attack(UnitController target)
        {
            targetUnit = target.unit;
            StartCoroutine(Move(target.transform.position, true));
            //StartCoroutine(FaceOponent(target.transform.position));
            isAttacker = true;
        }     
        
        public void DealDamage()
        {
            this.transform.LookAt(targetUnit.controller.transform.position);
            inCombat = true;
            anim.SetBool("inBrawl", true);           
            float x;

            if (isAttacker == true)
            {
                x = 1.0f;
            }
            else
            {
                x = 0.5f;
            }

            float damageDone = broker.combatSpeed * (attackPower * (attackRate / 10f)) - (targetUnit.controller.defence * x);
            targetUnit.health -= damageDone;
            Debug.Log(targetUnit.health);

            if (targetUnit.health <= 0f)
            {
                targetUnit.controller.Die();
                if (brawl) brawl.RemoveUnit(targetUnit.controller);
                targetUnit = null;
                isAttacker = false;
                inCombat = false;
                anim.SetBool("inBrawl", false);
            }

            //TODO give damage to enumerator
            //TODO deal it to enemy
        }

       /* IEnumerator FaceOponent(Vector3 target)
        {           
            {
                this.transform.LookAt(targetUnit.controller.transform.position);

                var lastRot = transform.rotation.y;

                while (inCombat == true)
                {
                    var rot = transform.rotation.y - lastRot;
                    anim.SetFloat("turning", rot);
                    lastRot = transform.rotation.y;
                    yield return null;
                }
            }
        }*/

        public void FightAnimation()
        {                        
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
        *     TIME SENSITIVE
        ===================================*/
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
    }
}
