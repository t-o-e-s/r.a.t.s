using System;

namespace Library.src.combat.Weapon
{
    [Serializable]
    public class Weapon
    {
        public int id;
        public string name;
        public float damage;
        public float range;
        public float speed;
        public WeaponType weaponType;
    }
}
