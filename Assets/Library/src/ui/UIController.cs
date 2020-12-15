using System;
using System.Collections;
using System.Collections.Generic;
using Library.Units;
using UnityEngine;
using UnityEngine.UI;

public class UIController
{
    Unit unit;
    GameObject panel;
    
    Text name;
    Text health;
    Text combat;

    string nameStr;
    string healthStr;
    string combatStr;
    string[] combatIndicator = new string[2] {"On", "Off"};
    

    public UIController(Unit unit, GameObject panel)
    {
        this.unit = unit;
        this.panel = panel;

        foreach (var t in panel.GetComponentsInChildren<Text>())
        {
            if (t.gameObject.name == "name")
            {
                name = t;
            }
            else if (t.gameObject.name == "health")
            {
                health = t;
                healthStr = t.text;
            }
            else if (t.gameObject.name == "combat")
            {
                name = t;
                combatStr = t.text;
            }
        }
    }

    public void UpdateUI()
    {
        name.text = unit.name;
        health.text = String.Format(healthStr, unit.health);
        combat.text = String.Format(combatStr, combatIndicator[unit.isFighting ? 1 : 0]);
    }
}
