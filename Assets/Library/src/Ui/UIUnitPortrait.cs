using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.management.units;
using Library.src.units.control;
using UnityEngine.UI;


public class UIUnitPortrait : MonoBehaviour
{    
    UnitController unitCon;
    public Image hpGrn;
    public Image portraitBg;

    private float maxHp = 100.0f;
    private float currentHp;

    private void Awake()
    {
        hpGrn = GameObject.Find("health_bg_green").GetComponent<Image>();
        unitCon = GameObject.FindGameObjectWithTag("player_unit").GetComponentInChildren<UnitController>();
        portraitBg = GameObject.Find("rat_portrait_HL").GetComponent<Image>();
    }
    void Update()
    {
        currentHp = unitCon.unit.health;
        hpGrn.transform.localScale = new Vector3(currentHp / maxHp, 1.0f, 1.0f);

        if (unitCon.flagEnabled == true)
        {
            portraitBg.enabled = true;
        }
        else if (unitCon.flagEnabled == false)
        {
            portraitBg.enabled = false;
        }

    }
}
