using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;

namespace Library.src.units
{
    public class Unit
    {
        //Unit info
        public string name;
        public UnitController controller;
        
        //Stats
        public float health;
        public float speed;
        public Status[] statuses;
        
        //Combat fields
        public Brawl brawl;
        public Weapon weapon;

    }
}
