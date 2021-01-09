using System.Collections;
using Library.src.combat;
using Library.src.ui;
using Library.src.util;
using UnityEngine;
using UnityEngine.AI;

namespace Library.src.units
{
    public class UnitController : MonoBehaviour, IUnitController, ITimed
    {
        public Unit unit;
        public ICombat combat;
        [HideInInspector]
        public bool playerUnit;

        //nav agent and related fields
        protected NavMeshAgent agent;
        [Header("Navigation")]
        [SerializeField]
        protected float slowedSpeed = 5;

        Animator anim;
        Broker broker;
        //sprite above the unit to dictate status
        SpriteRenderer flag;
        UIController ui;
    
        private void Awake()
        {
            playerUnit = CompareTag("player_unit");
        
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            broker = Camera.main.gameObject.GetComponent<Broker>();
            flag = GetComponentInChildren<SpriteRenderer>();

            broker.Add(this);
            broker.LoadThis(this);
        }

        private void Update()
        {
            //TODO implement through an InvokeRepeating() and test it, might be an easy way to claw some frames back
            SamplePosition();
        }

        /*====================================
     *     COMBAT
     ===================================*/
        public bool Attack(UnitController target)
        {
            if (combat == null) combat = new combat.Combat(broker, this, target, false);
            else ; //TODO what if unit is already engaged? -> combat.CanBreak() 
            //informing defending unit
            if (target.combat == null)
            {
                target.combat = new combat.Combat(broker, target, this, true);
                target.combat.SetMutual(true);
                combat.SetMutual(true);
            }
            return true;
        }

        public void WaitForAttacker()
        {
            StartCoroutine(WaitForAttacker(combat.GetOpponent()));

        }

        IEnumerator WaitForAttacker(UnitController attacker)
        {
            while (Vector3.Distance(attacker.transform.position, gameObject.transform.position) > broker.stoppingDistance)
            {
                yield return null;
            }
            combat.SetReady(true);
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
            anim.SetBool("move", true);
            var lastRot = transform.rotation.y;
            
            while (Vector3.Distance(target, transform.position) > broker.stoppingDistance)
            {
                var rot = transform.rotation.y - lastRot;
                anim.SetFloat("turning", rot);
                lastRot = transform.rotation.y;
                yield return null;
            }

            anim.SetBool("move", false);
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
   *     NAVIGATION
   ===================================*/

        public void FetchLoot(Vector3 target)
        {
            StartCoroutine(MoveToLoot(target));
        }

        IEnumerator MoveToLoot(Vector3 target)
        {
            agent.SetDestination(target);
            while (agent.remainingDistance > broker.stoppingDistance)
            {
                yield return null;
            }
            agent.SetDestination(agent.transform.position);
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
            this.unit = unit;
        }
        
        /*====================================
     *     UI
     ===================================*/
        public void UpdateUI(GameObject panel)
        {
            ui = new UIController(unit, panel);
        }
        
        public void UpdateUI()
        {
            ui.UpdateUI();
        }
        
        /*====================================
     *     INFO
     ===================================*/
        public float GetSpeed()
        {
            return agent.speed;
        }
    }
}
