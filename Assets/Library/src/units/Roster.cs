using System.Collections;
using System.Collections.Generic;
using Library.src.units;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public readonly Unit[] activeUnits;
    public readonly Unit[] warband;

    public Roster(ICollection<IUnitController> activeUnits, ICollection<IUnitController> warband)
    {
        this.activeUnits = new Unit[activeUnits.Count];
        this.warband = new Unit[warband.Count];

        var i = 0; 


        foreach (var unitController in activeUnits)
        {
            var unit = (UnitController) unitController;
            this.activeUnits[i++] = unit.GetUnit();
        }

        i = 0;

        foreach (var unitController in warband)
        {
            var unit = (UnitController) unitController;
            this.warband[i++] = unit.GetUnit();
        }
    }

}
