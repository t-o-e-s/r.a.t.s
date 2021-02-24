using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.util;
using Library.src.units;

public class PlayerUnitStateMachine : MonoBehaviour
{
    UnitController unitCon;

    void Awake()
    {
        unitCon = GetComponent<UnitController>();      
    }
       
    /*private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == EnvironmentUtil.TAG_AI)
        {
            unitCon.unitClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != EnvironmentUtil.TAG_AI)
        {
            unitCon.unitClose = false;
        }
    }*/

}
