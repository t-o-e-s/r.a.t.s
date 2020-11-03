using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class UnitSelection : MonoBehaviour
{
    public Image selectInd;
    public bool unitSelected;
    NavMeshAgent agent;
    private void Awake()
    {
        selectInd.enabled = false;
        unitSelected = false;
        selectInd = GetComponentInChildren<Image>();
        agent = GetComponent<NavMeshAgent>();

    }

    private void OnMouseOver()
    {
        if (unitSelected == false)
        {
            selectInd.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (unitSelected == false)
        {
            selectInd.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (unitSelected == false)
        {
            selectInd.enabled = true;
            unitSelected = true;
        }
        else
        {
            selectInd.enabled = false;
            unitSelected = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "slope")
        {
            agent.speed = 5;
        }
        else
        {
            agent.speed = 10;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "slope")
        {
            agent.speed = 10;
        }
    }

    

 
}
