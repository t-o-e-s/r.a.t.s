using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsNode_Proto : MonoBehaviour
{

    RtsMovement_Proto rtsMove;
    public bool nodeSelected;
    public Vector3 nodePos; 

    private void OnMouseDown()
    {
        if (rtsMove.unitSelected == true)
        {
            nodeSelected = true;
        }
    }


    private void Start()
    {
        nodePos = gameObject.transform.position;
        rtsMove = GameObject.FindGameObjectWithTag("Unit").GetComponent<RtsMovement_Proto>();
    }
}
