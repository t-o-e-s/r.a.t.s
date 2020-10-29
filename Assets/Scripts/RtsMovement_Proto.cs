using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RtsMovement_Proto : MonoBehaviour
{
    float movementSpeed;
    public bool unitSelected;
    bool canMove;
    bool unitMoving;
    Canvas can;
    public Image img;
    public Vector3 targetPosition;
    RtsNode_Proto rtsNode;
    GameObject[] nodeArray;

    void Start()
    {
        unitSelected = false;
        unitMoving = false;
        can = GetComponent<Canvas>();
        img.enabled = false;
        movementSpeed = 10f;

        nodeArray = GameObject.FindGameObjectsWithTag("Node");
       
    }

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

    private void OnMouseDown()
    {
        if (gameObject.tag == "Unit")
        {
            unitSelected = true;
        }
       
    }

    private void Update()
    {
        foreach (GameObject node in nodeArray)
        {

            if (node.gameObject.GetComponent<RtsNode_Proto>().nodeSelected == true)
            {
                targetPosition = node.gameObject.GetComponent<RtsNode_Proto>().nodePos;
                canMove = true;
                
                if(node.gameObject.GetComponent<RtsNode_Proto>().unitArrived == true)
                {
                    canMove = false;
                    unitSelected = false;
                    unitMoving = false;
                }
            }
        }

        

         if(unitSelected == true && canMove == true)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
            unitMoving = true;
        }

    }


}
