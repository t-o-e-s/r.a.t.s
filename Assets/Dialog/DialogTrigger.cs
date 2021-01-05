using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Library.src.units;

public class DialogTrigger : MonoBehaviour
{
    DialogDisplay dialogDisplay;
    

    private void Awake()
    {
        dialogDisplay = GameObject.FindGameObjectWithTag("canvas").GetComponentInChildren<DialogDisplay>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player_unit")
        {
            dialogDisplay.convoTriggered = true;
            other.gameObject.GetComponent<UnitController>().StopAnimation();
        }
    }
}
