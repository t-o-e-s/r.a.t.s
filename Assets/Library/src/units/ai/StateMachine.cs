using Library.src.units.control;
using Library.src.util;
using UnityEngine;

namespace Library.src.units.ai
{
    public class StateMachine : MonoBehaviour
    {
        [Header("Patrol Behaviour")]
        public GameObject[] patrolNodes;
        
        protected UnitController controller; //unitController of the object controlled by the SM
        protected UnitController enemyInVision;

        AIState state = AIState.IDLE;

        bool on; //state machine will only work if this is on
        bool canPatrol;

        int patrolIndex = 0;

        string enemyTag;

        /*
         *  ========================================
         *  BASE
         *  ========================================
         */
        
        void Awake()
        {
            var attachedToPlayer = CompareTag(EnvironmentUtil.TAG_PLAYER);
            
            controller = GetComponent<UnitController>();
            on = !attachedToPlayer;
            enemyTag = attachedToPlayer ? EnvironmentUtil.TAG_AI : EnvironmentUtil.TAG_PLAYER;
            canPatrol = patrolNodes.Length > 1;

            UpdateState();
        }

        void Update()
        {
            if (!on) return;
            if (controller.InCombat()) return;
            if (state == AIState.PATROL && controller.agent.isStopped) 
                UpdateState();
        }

        void UpdateState()
        {
            if (!(enemyInVision is null))
            {
                CombatBehaviour();
            }
            else if ((!controller.agent.pathPending && canPatrol)
                || controller.GetTarget() is null && canPatrol && state == AIState.IDLE)
            {
                if (patrolNodes.Length == 0)
                {
                    Idle();
                    return;
                }
                state = AIState.PATROL;
                Patrol();
            }
        }
        
        /*
         *  ========================================
         *  STATE BEHAVIOURS
         *  ========================================
         */

        void Patrol()
        {
            if (controller.agent.pathPending) return;
            patrolIndex = patrolIndex >= (patrolNodes.Length - 1) ? 0 : patrolIndex++;
            var patrolTarget = patrolNodes[patrolIndex];
            controller.Move(patrolTarget.transform.position);
        }

        void CombatBehaviour()
        {
            //if the enemy in vision is attacking this unit then defend
            if (enemyInVision.unit.charging 
                && !(enemyInVision.GetTarget() is null)
                && (enemyInVision.GetTarget().controller == controller))
            {
                Defend();

            }

            else Attack();
        }

        void Attack()
        {
            state = AIState.ATTACK;
            controller.Attack(enemyInVision);
        }

        void Defend()
        {
            state = AIState.DEFEND;
            controller.LookAt(enemyInVision.transform.position);
        }

        void Idle()
        {
            state = AIState.IDLE;
            controller.Halt();
        }
        
        /*
         *  ========================================
         *  EVENTS
         *  ========================================
         */

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag(enemyTag)) return;
            if (other.TryGetComponent(out enemyInVision)) CombatBehaviour();
        }
    }
}