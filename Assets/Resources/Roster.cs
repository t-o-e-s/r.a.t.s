using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Roster
{
    public HashSet<GameObject> activeUnits = new HashSet<GameObject>();
    public HashSet<GameObject> warBand = new HashSet<GameObject>();         
}
