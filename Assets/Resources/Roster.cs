using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public HashSet<UnitController> activeUnits = new HashSet<UnitController>();
    public HashSet<UnitController> warband = new HashSet<UnitController>();


}
