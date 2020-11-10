using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroup
{
    //health
    public float health;
    //attack & defense
    public float attackPower;
    public float defensePower;
    //modifier can be set through customisation
    public float modifier;

    public UnitGroup(HashSet<UnitController> units)
    {

    }

    public void TakeDamage(float damage)
    {
        
    }
}
