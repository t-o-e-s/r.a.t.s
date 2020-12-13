using Library.Combat;
using Library.Combat.Weapon;

namespace Library.Units
{
    public struct Unit
    {
        public float health;
        public float speed;
        public Weapon weapon;
        public Status[] statuses;
    }
}
