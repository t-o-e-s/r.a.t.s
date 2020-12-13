using System.Collections;
using System.Collections.Generic;
using Library.src.units;
using Library.Units;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public readonly Unit[] activeUnits;
    public readonly Unit[] warband;

    public Roster(HashSet<IUnitController> activeUnits, HashSet<IUnitController> warband)
    {
        this.activeUnits = new Unit[activeUnits.Count];
        this.warband = new Unit[warband.Count];

        int i = 0; 


        foreach (UnitController unit in activeUnits)
        {
            this.activeUnits[i++] = unit.GetUnit();
        }

        i = 0;

        foreach (UnitController unit in warband)
        {
            this.warband[i++] = unit.GetUnit();
        }
    }

}
