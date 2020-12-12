using UnityEngine;

namespace Library.Units
{
    public interface IUnitController
    {
        void LoadAs(Unit unit);
        void Attack(UnitControllerController target);
        void Flag(bool flag);
        void Move(Vector3 target);
        bool InCombat();
    }
}
