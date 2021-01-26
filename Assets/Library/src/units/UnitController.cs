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
        //healthbar object above the unit to dictate health, help pls
        Canvas healthBar;
        
        //combat related fields
        Brawl brawl = null;
        Unit targetUnit;
        [SerializeField] float defence;
        bool isAttacker;
        bool inCombat;

        
        //time fields
        bool isForwarding = false;
        bool isRewinding = false;


        bool inVisionCone;
        bool isMoving;


        void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            broker = Camera.main.gameObject.GetComponent<Broker>();

            broker.Add(this);
            broker.LoadAs(this);

            playerUnit = CompareTag("player_unit");
        
            anim = GetComponent<Animator>();
            healthBar = GetComponentInChildren<Canvas>();

            targetUnit = null;

            io = Camera.main.GetComponent<IOHandler>();

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
            StartCoroutine(AttackMove(target.transform.position, true));
            inCombat = true;
            isAttacker = true;
        }     
        
        public void DealDamage()
        {
            isMoving = false;
            //this.transform.LookAt(targetUnit.controller.transform.position);
            FightAnimation();
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

            float damageDone = broker.combatSpeed * (unit.weapon.damage * (unit.weapon.speed / 10f)) - (targetUnit.controller.defence * x);
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
        IEnumerator AttackMove(Vector3 target, bool toAttack)
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

            anim.SetBool("inBrawl", true);
            anim.SetBool("move", false);
            agent.SetDestination(agent.transform.position);

            if (toAttack && !InCombat()) InitiateBrawl();
        }

        public void FightAnimation()      
        {

            anim.SetTrigger("isSlashing");
        }
        

        public void Flag(bool flag)
        {
            this.healthBar.enabled = flag;
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

        public void LookAtUnit(UnitController unit)
        {
            if (inVisionCone == true)
            {
                StartCoroutine(Turn(unit));
                this.transform.LookAt(unit.transform.position);               
            }
            
        }

        IEnumerator Turn(UnitController unit)
        { 

            while (inVisionCone == true && unit.isMoving == true)
            {   
                anim.SetFloat("turning", 1f);
                
                yield return null;
            }

            
            anim.SetFloat("turning", 0f);

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
            isMoving = true;
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
            isMoving = false;
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

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.CompareTag("loot"))
            {
                
                Destroy(other.gameObject);
                anim.SetBool("move", false);
                agent.SetDestination(agent.transform.position);
                io.TakeLoot();
            }

            if(other.gameObject.CompareTag("player_unit"))
            {
                inVisionCone = true;
                LookAtUnit(other.GetComponent<UnitController>());
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("player_unit"))
            {
                inVisionCone = false;
               
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
