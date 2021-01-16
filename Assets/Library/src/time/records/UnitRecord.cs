using Library.src.units;
using UnityEngine;

namespace Library.src.time.records
{
    public class UnitRecord : Record
    {
        public readonly Transform transform;

        public readonly int id;

        public UnitRecord(Unit unit)
        {
            id = unit.controller.gameObject.GetInstanceID();
            transform = unit.controller.transform;
        }
    }
}