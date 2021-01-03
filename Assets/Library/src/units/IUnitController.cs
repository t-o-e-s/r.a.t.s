using System.Collections;
using UnityEngine;

namespace Library.src.units
{
    public interface IUnitController
    {
        Unit GetUnit();
        void LoadAs(Unit unit);
        void Attack(UnitController target);
        GameObject GetOccupyingTile();
        void Flag(bool flag);
        void MoveTo(Vector3 target);
        void UpdateUI(GameObject panel);
        void UpdateUI();
    }
}
