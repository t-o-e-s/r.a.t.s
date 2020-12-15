using Library.Combat;
using Library.Combat.Weapon;

namespace Library.Units
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
