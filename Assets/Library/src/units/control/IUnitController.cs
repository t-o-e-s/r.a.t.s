using Library.src.units.control;
using UnityEngine;

namespace Library.src.units
{
    public interface IUnitController
    {
        //Information & Utility
        Unit GetUnit();
        Unit GetTarget();
        void Flag(bool flag);
        void Die();
        
        //Combat
        void Attack(UnitController target);
        void DealDamage();
        void SetTarget(Unit unit);

        //Navigation
        void Move(Vector3 target);
        void Follow(Transform target);
    }
}
