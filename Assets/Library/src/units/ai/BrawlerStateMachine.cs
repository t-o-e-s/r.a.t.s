using Library.src.util;
using UnityEngine;

namespace Library.src.units.ai
{
    public class BrawlerStateMachine : MonoBehaviour
    {
   
        UnitController unitCon;  

        //patrol related fields 
        public GameObject[] nodeArray;
        private int node = 0;
    
        void Awake()
        {       
            unitCon = GetComponent<UnitController>();
            PatrolBehaviour();
        
        }

        // Update is called once per frame
        void Update()
        {
            if (unitCon.inCombat == false)
            {
                if (!unitCon.agent.pathPending && unitCon.agent.remainingDistance < 0.5f)
                    PatrolBehaviour();
            }
        }


        void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case EnvironmentUtil.TAG_PLAYER:
                    CombatBehaviour(other.gameObject.GetComponent<UnitController>());
                    Debug.Log("in combat");
                    break;
            }
        }
    
        void PatrolBehaviour()
        {
            if (nodeArray.Length == 0)
                return;

            unitCon.MoveTo(nodeArray[node].transform.position);

            node = (node + 1) % nodeArray.Length;
        }

        void CombatBehaviour(UnitController playerUnit)
        {
            //unitCon.Halt();

            if (playerUnit.isAttacker == false)
            {
                Debug.Log("UnitAttacking");
                unitCon.Attack(playerUnit);
            }
            else if (playerUnit.isAttacker == true)
            {
                Debug.Log("UnitDefending");
                unitCon.inCombat = true;
                unitCon.MoveTo(unitCon.agent.transform.position);
                unitCon.LookAtUnit(playerUnit.transform.position);
            }
        }  

    }
}
