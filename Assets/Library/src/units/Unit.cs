﻿using Library.src.animation;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;
using Library.src.management.units;
using Library.src.units.control;

namespace Library.src.units
{
    public class Unit
    {
        //Unit info ------------------------------------------------
        public string name;
        public UnitController controller;
        
        //Stats ----------------------------------------------------
        public float health;
        public float speed;
        public Status[] statuses;
        
        //Combat fields --------------------------------------------
        public Brawl brawl;
        public Weapon weapon;
        public bool charging;
        public float attackPower = 1f;
        public float attackRate = 1f;
        public float defence = 0f;
        
        //Animation ------------------------------------------------
        public AnimationHandler animator;

        Unit (Unit unit)
        {
            name = unit.name;
            controller = unit.controller;
            health = unit.health;
            speed = unit.speed;
            statuses = unit.statuses;
            brawl = unit.brawl;
            weapon = unit.weapon;
            animator = unit.animator;
        }

        Unit (string name, 
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

        public static Unit CreateUnit(UnitController unitController, UnitLoadData loadData, Weapon weapon)
        {
            var unit = new Unit(
                unitController.name,
                unitController,
                loadData.health,
                loadData.speed,
                new Status[12],
                null,
                weapon);
            unit.animator = new AnimationHandler(unit);
            return unit;
        }
    }
}
