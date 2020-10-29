using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RtsMovement_Proto : MonoBehaviour
{

    float movementSpeed;
    public bool unitSelected;
    public bool canMove;
   
    public bool unitMoving;
    
    //simple image just for visual indication of selection
    public Image img;
    //array containing all movement nodes
    GameObject[] nodeArray;
    //assignable vector3 for targetpos for unit to move to 
    public Vector3 targetPosition;

    void Start()
    {
        unitSelected = false;
        unitMoving = false;
        img.enabled = false;
        movementSpeed = 10f;

        nodeArray = GameObject.FindGameObjectsWithTag("Node");
       
    }

    //displaying that you can select the unit
    private void OnMouseOver()
    {
        img.enabled = true;
    }

    private void OnMouseExit()
    {
        if (unitSelected == false)
        {
            img.enabled = false;
        }
        else
        {
            img.enabled = true;
        }
    }
       
    //for selecting the unit, just triggers the bool 
    private void OnMouseDown()
    {
        if (gameObject.tag == "Unit")
        {
            unitSelected = true;
        }
       
    }

    private void Update()
    {
        //gets each node in the arry and checks if the script attached to it has been trigger by the player 
        foreach (GameObject node in nodeArray)
        {

            if (node.gameObject.GetComponent<RtsNode_Proto>().nodeSelected == true && unitMoving == false)
            {
                targetPosition = node.gameObject.GetComponent<RtsNode_Proto>().nodePos;
                canMove = true;

                //when unit reaches node it resets the unit to default
                if (node.gameObject.GetComponent<RtsNode_Proto>().unitArrived == true)
                {
                    canMove = false;
                    img.enabled = false;
                    unitMoving = false;
                }
            }           
        }

        
        //move the unit to the selected node
         if(unitSelected == true && canMove == true)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
            unitMoving = true;
        }

    }


}
