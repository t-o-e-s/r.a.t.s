using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.combat;
using Library.src.util;
using Library.src.units;



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
        if (!unitCon.agent.pathPending && unitCon.agent.remainingDistance < 0.5f && unitCon.inCombat == false)
             PatrolBehaviour();

    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case null:
                PatrolBehaviour();
                break;
            case "player_unit":
                PursueBehaviour(other.gameObject.GetComponent<UnitController>());
                break;
        }           
    }

    public void PatrolBehaviour()
    {
        if (nodeArray.Length == 0)
            return;

        unitCon.MoveTo(nodeArray[node].transform.position);

        node = (node + 1) % nodeArray.Length;
    }

    public void PursueBehaviour(UnitController playerUnit)
    {
        if (playerUnit.isAttacker == false)
        {
            unitCon.Attack(playerUnit);
        }
        else if (playerUnit.isAttacker == true)
        {
            unitCon.inCombat = true;
            unitCon.MoveTo(unitCon.agent.transform.position);
            unitCon.LookAtUnit(playerUnit);
        }
    }

   

}
