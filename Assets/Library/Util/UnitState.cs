using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitState : State
{
    //this just calls the parent classes contructor with the same fields
    public UnitState(GameObject gameObject, Vector3 position, Quaternion rotation, float health, Status[] statuses, bool inCombat) 
        : base(gameObject, position, rotation, health, statuses, inCombat)
    {
    }
    
}
