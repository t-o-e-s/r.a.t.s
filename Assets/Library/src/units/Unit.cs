using Library.src.combat.Weapon;
using Library.src.elements;

namespace Library.src.units
{
    public struct Unit
    {
        public string name;
        public bool isFighting;
        public float health;
        public float speed;
        public Weapon weapon;
        public Status[] statuses;
    }
}
