using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimeHandler : MonoBehaviour
{
    //list for storing unit positions
    public List<Vector3> unitPositions;
    //for setting the pos of the unit during rewind 
    private Vector3 destination;

    //unit the script is attached to, and its unitController 
    private GameObject unit;
    UnitController unitCont; 

    //is the player rewinding time, currently set to a hold system (like braid)
    public bool isRewinding;  
     
    private void Awake()
    {
        unit = gameObject;
        unitCont = GetComponent<UnitController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isRewinding = true;
        }
        else
        {
            isRewinding = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            //storing the position of the unit every fixedUpdate
            unitPositions.Add(unit.transform.position);
        }
        else
        {
            //setting destination to be the current pos in the List, then removing the positions from the list sequentially 
            destination = (Vector3)unitPositions[unitPositions.Count - 1];
            unitPositions.RemoveAt(unitPositions.Count - 1);

            //accessing the unit controller to set the destination of the agent to previous positions
            unitCont.MoveTo(target: destination);
        }
    }
}
