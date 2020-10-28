using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RtsMovement_Proto : MonoBehaviour
{

    Transform unitPosition;
    float movementSpeed;
    public bool unitSelected;
    bool unitMoving;
    Canvas can;
    public Image img;
    Vector3 targetPosition;
    RtsNode_Proto rtsNode;

    void Start()
    {
        unitSelected = false;
        unitMoving = false;
        can = GetComponent<Canvas>();
        img.enabled = false;
        movementSpeed = 10f;
        rtsNode = GameObject.FindGameObjectWithTag("Node").GetComponent<RtsNode_Proto>();
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
        if(rtsNode.nodeSelected == true)
        {
            targetPosition = rtsNode.nodePos;
        }

         if(unitSelected == true && rtsNode.nodeSelected == true)
        {
            this.gameObject.transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
            unitMoving = true;
        }

    }


}
