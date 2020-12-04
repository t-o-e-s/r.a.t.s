using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public readonly HashSet<UnitController> activeUnits = new HashSet<UnitController>();
    public readonly HashSet<UnitController> warband = new HashSet<UnitController>();

    public Roster(HashSet<UnitController> activeUnits, HashSet<UnitController> warband)
    { 
        this.activeUnits = activeUnits; 
        this.warband = warband; 
    }
}
