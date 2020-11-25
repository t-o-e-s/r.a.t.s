﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeController : UnitController
{
    public override void Attack(UnitController target) 
    {
        //calls unit controller base method
        base.Attack(target);
        //starting movement co-routine
        StartCoroutine(MoveToAttack(target));
    }

    public override void SetStats()
    {
        //TODO
    }   
}
