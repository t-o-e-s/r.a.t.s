using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library.src.units
{
    public interface IUnitController
    {
        //Information & Utility
        Unit GetUnit();
        void LoadAs(Unit unit);
        
        //Combat
        void Attack(UnitController target);
        void DealDamage();
        void SetTarget(Unit? unit);

        //Navigation
        GameObject GetOccupyingTile();
        void Flag(bool flag);
        void MoveTo(Vector3 target);
        
        //UI
        void UpdateUI(GameObject panel);
        void UpdateUI();
    }
}
