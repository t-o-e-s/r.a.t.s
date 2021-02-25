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

        Unit(Unit unit)
        {
            name = unit.name;
            controller = unit.controller;
            health = unit.health;
            speed = unit.speed;
            statuses = unit.statuses;
            brawl = unit.brawl;
            weapon = unit.weapon;
        }

        public Unit(string name, 
            UnitController controller, 
            float health, 
            float speed, 
            Status[] statuses, 
            Brawl brawl, 
            Weapon weapon)
        {
            this.name = name;
            this.controller = controller;
            this.health = health;
            this.speed = speed;
            this.statuses = statuses;
            this.brawl = brawl;
            this.weapon = weapon;
        }
    }
}
