using UnityEngine;

namespace Library.Units
{
    public interface IUnitController
    {
        Unit GetUnit();
        void LoadAs(Unit unit);
        void Attack(UnitController target);
        void Flag(bool flag);
        void MoveTo(Vector3 target);
        bool InCombat();
    }
}
