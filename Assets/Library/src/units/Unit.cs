using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;

namespace Library.src.units
{
    public struct Unit
    {
        //Unit info
        public string name;
        public IUnitController controller;
        
        //Stats
        public float health;
        public float speed;
        public Status[] statuses;
        
        //Combat fields
        public ICombat combat;
        public Weapon weapon;
    }
}
