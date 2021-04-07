using System.Collections;
using Library.src.animation;
using Library.src.combat;
using Library.src.time;
using Library.src.time.records;
using Library.src.util;
using UnityEngine;
using UnityEngine.AI;

namespace Library.src.units.control
{
    public class UnitController : MonoBehaviour, IUnitController, ITimeSensitive
    {
        //Unit Model ------------------------------------------
        [HideInInspector] public Unit unit;
        
        //Animation -------------------------------------------
        AnimationHandler animator;
        
        //Navigation ------------------------------------------
        [HideInInspector] public NavMeshAgent agent;

        //Scene -----------------------------------------------
        Broker broker;
        
        //Combat ----------------------------------------------
        Brawl brawl;
        Unit targetUnit;
        //TODO reimplement a way to load unit values in from an xml

        //UI --------------------------------------------------
        SpriteRenderer flag; //TODO move this into a UIHandler
        
        //Routines ---------------------------------------------
        Coroutine movementRoutine;
        Coroutine lookRoutine;

        void Awake()
        {
            var mainCamera = Camera.main;
            if (mainCamera is null) {} //TODO implement scene quit

            broker = mainCamera.GetComponent<Broker>();
            
            agent = GetComponent<NavMeshAgent>();
            flag = GetComponentInChildren<SpriteRenderer>(); //TODO move to a UIHandler

            broker.Load(this);
            animator = new AnimationHandler(unit);
        }

         /*====================================
        *     COMBAT
        ===================================*/
        public void Attack(UnitController target)
        {
            Halt();
            targetUnit = target.unit;           
            
            var targetTransform = target.transform;
            movementRoutine = StartCoroutine(MoveRoutine(
                targetTransform,
                targetTransform.position,
                true));
        }     
        
        public void DealDamage()
        {
            animator.Brawl(true);  
            animator.Slash();
            
            //setting charge bonus
            var x = 0.5f;
            if (unit.charging)
            {
                x = 1.0f;
            }
            
            var damage = unit.weapon.damage * (unit.weapon.speed / 10f); //damage done by this unit
            damage -= targetUnit.defence * x; //protection of the unit applied
            targetUnit.health -= damage; //damage dealt

            if (!(targetUnit.health <= 0f)) return; //if target is still alive we're done here
            
            animator.Brawl(false);
            targetUnit.controller.Die();
            brawl.RemoveUnit(targetUnit.controller);
            targetUnit = null;
            Debug.Log(unit.health);
        }

        public void LookAt(Vector3 target)
        {
            lookRoutine = StartCoroutine(LookRoutine(target));
        }

       IEnumerator LookRoutine(Vector3 target)
       {
           yield return null;
           //TODO this is just wrong
           /*{
               Quaternion _lookRotation = Quaternion.LookRotation((target - transform.position).normalized);
               
               while (this.inCombat == true)
               {
                   Debug.Log("turn");
                   float turn_speed = 2f;
                   transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turn_speed);
                   animator.Turn(1f);                 
                   yield return null;
               }
           }*/
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
        public void Move(Vector3 target)
        {
            Halt();
            movementRoutine = StartCoroutine(MoveRoutine(null, target, false));
        }
        
        public void Follow(Transform target)
        {
            movementRoutine = StartCoroutine(MoveRoutine(target, target.position, false));
        }

        IEnumerator MoveRoutine(Transform target, Vector3 targetPos, bool toAttack)
        {
            if (!(lookRoutine is null))
            {
                StopCoroutine(lookRoutine);
                lookRoutine = null;
            }
            
            unit.charging = toAttack;
            
            var stoppingDistance = toAttack ? unit.weapon.range : EnvironmentUtil.STOPPING_DISTANCE;
            var lastRotation = transform.rotation.y;
            targetPos = !(target is null) ? target.position : targetPos;

            agent.isStopped = false;

            agent.SetDestination(targetPos);
            animator.Move( true);

            while (Vector3.Distance(targetPos, transform.position) > stoppingDistance)
            {
                targetPos = target != null ? target.position : targetPos;
                agent.SetDestination(targetPos);
                var currentRotation = transform.rotation.y;
                var rotation = currentRotation - lastRotation;
                animator.Turn(rotation);
                lastRotation = currentRotation;
                yield return null;
            }

            animator.Move(false);
            agent.SetDestination(agent.transform.position);
            agent.isStopped = true;

            if (toAttack && brawl is null) InitiateBrawl();
        }        

        public void Halt()
        {
            animator.Move( false);
            if (!(movementRoutine is null)) StopCoroutine(movementRoutine);
            if (!(lookRoutine is null)) StopCoroutine(lookRoutine);
            unit.charging = false;
            targetUnit = null;
            agent.SetDestination(transform.position);
        }

        /*====================================
        *     Loot
        ===================================*/

        public void FetchLoot(Vector3 target)
        {
            //TODO there should be another check to confirm if we've got the RIGHT loot.
           movementRoutine = StartCoroutine(MoveRoutine(null, target, false));
        }

        //TODO loot objects should handle their own looting not the UC
        /*void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("loot"))
            {
                Destroy(other.gameObject);
                animator.Move(false);
                agent.SetDestination(agent.transform.position);
                io.TakeLoot();
            }
        }*/


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

        public bool InCombat()
        {
            return !(targetUnit is null) || !(brawl is null);
        }
        
        /*====================================
        *     INFO
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
    }
}
