using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library.src.units
{
    public interface IUnitController
    {
        //Information & Utility
        Unit GetUnit();
        Unit GetTarget();
        void LoadAs(Unit unit);
        void Flag(bool flag);
        void Die();
        
        //Combat
        void Attack(UnitController target);
        void DealDamage();
        void SetTarget(Unit unit);

        //Navigation
        void MoveTo(Vector3 target);
    }
}
