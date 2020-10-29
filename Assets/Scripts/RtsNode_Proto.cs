using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsNode_Proto : MonoBehaviour
{

    RtsMovement_Proto rtsMove;
    public bool nodeSelected;
    public Vector3 nodePos;
    public bool unitArrived;
    private void OnMouseDown()
    {
        if (rtsMove.unitSelected == true)
        {
            nodeSelected = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Unit")
        {
            unitArrived = true;
            nodeSelected = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Unit")
        {
            unitArrived = false;
        }
    }

    private void Start()
    {
        nodePos = gameObject.transform.position;
        rtsMove = GameObject.FindGameObjectWithTag("Unit").GetComponent<RtsMovement_Proto>();
    }
}
