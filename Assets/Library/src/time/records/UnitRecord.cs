using Library.src.combat.Weapon;
using Library.src.elements;
using Library.src.units;
using UnityEngine;

namespace Library.src.time.records
{
    public class UnitRecord : Record
    {
        public readonly Vector3 position;
        public readonly Weapon weapon;

        public readonly Status[] statuses;

        public readonly float health;
        public readonly float speed;


        public UnitRecord(Unit unit) : base(unit.controller.gameObject.GetInstanceID(), unit.name)
        {
            var t = unit.controller.transform.position;
            position = new Vector3(t.x, t.y, t.z);
            weapon = unit.weapon;

            statuses = unit.statuses;

            health = unit.health;
            speed = unit.speed;
        }
    }
}