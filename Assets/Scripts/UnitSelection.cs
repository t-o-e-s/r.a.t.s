using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    public Image selectInd;
    public bool unitSelected;

    private void Awake()
    {
        selectInd.enabled = false;
        unitSelected = false;
        selectInd = GetComponentInChildren<Image>();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
