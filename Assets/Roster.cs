using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public readonly Rat[] activeUnits;
    public readonly Rat[] warband;

    public Roster(HashSet<UnitController> activeUnits, HashSet<UnitController> warband)
    {
        this.activeUnits = new Rat[activeUnits.Count];
        this.warband = new Rat[warband.Count];

        int i = 0; 


        foreach (UnitController unit in activeUnits)
        {
            this.activeUnits[i++] = unit.GetRat();
        }

        i = 0;

        foreach (UnitController unit in warband)
        {
            this.warband[i++] = unit.GetRat();
        }
    }

}
