using System.Collections;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.io;
using Library.src.ui;
using Library.src.util;
using UnityEngine;
using UnityEngine.AI;

namespace Library.src.units
{
    public class UnitController : MonoBehaviour, IUnitController, ITimed
    {
        [HideInInspector]
        public Unit unit;
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
    
        void Awake()
        {
            playerUnit = CompareTag("player_unit");
        
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            broker = Camera.main.gameObject.GetComponent<Broker>();
            flag = GetComponentInChildren<SpriteRenderer>();

            broker.Add(this);
            broker.LoadAs(this);
        }

        void Update()
        {
            //TODO implement through an InvokeRepeating() and test it, might be an easy way to claw some frames back
            SamplePosition();
        }

        /*====================================
        *     COMBAT
        ===================================*/
        void InitialAttack()
        {
            broker.Add(unit.combat as Combat);
            unit.combat.SetReady(true);
            
            if (ReferenceEquals(unit.combat.GetOpponent().combat, null))
            {
                if (unit.weapon.weaponType == WeaponType.Melee)
                {
                    var opponent = unit.combat.GetOpponent();
                    var tempCombat = new Combat(broker, unit.combat.GetOpponent(), unit);
                    tempCombat.SetMutual(true);
                    tempCombat.SetReady(true);
                    unit.combat.SetMutual(true);
                    opponent.combat = tempCombat;
                    broker.Add(tempCombat);
                }
            }
            else
            {
                //TODO attack piles on
            }
        }
        
        public void Attack(UnitController target)
        {
            if (ReferenceEquals(unit.combat, null))
            {
                unit.combat = new Combat(broker, unit, target.unit);
                GameObject targetTile = Locator.GetNearest(transform.position, target.GetOccupyingTile());
                
                //TODO an in range check will be needed
                if (unit.weapon.weaponType == WeaponType.Ranged) InitialAttack();
                else StartCoroutine(Move(targetTile.transform.position, true));
            }
            else if (Equals(unit.combat.GetOpponent(), target.unit))
            {
                //TODO calling attack on unit
                //TODO add combat to the broker for resolution
            }
            else
            {
                //TODO handle breaking off combat
            }
        } 

        public void Flag(bool flag)
        {
            this.flag.enabled = flag;
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

            if (toAttack) InitialAttack();
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
                unit.combat != null
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

        public GameObject GetOccupyingTile()
        {
            throw new System.NotImplementedException();
        }
    }
}
