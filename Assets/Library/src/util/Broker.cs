using System;
using System.Collections.Generic;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.management;
using Library.src.management.units;
using Library.src.time;
using Library.src.units;
using Library.src.units.control;
using UnityEngine;

namespace Library.src.util
{
    public class Broker : MonoBehaviour
    {
        //HashSets for tracking active units
        public readonly HashSet<IUnitController> units = new HashSet<IUnitController>();
        public readonly HashSet<IUnitController> playerUnits = new HashSet<IUnitController>();
        public readonly HashSet<IUnitController> hostileUnits = new HashSet<IUnitController>();
        //tracking time sensitive objects
        public readonly HashSet<ITimeSensitive> recordables = new HashSet<ITimeSensitive>();

        //units loaded in from json
        public Dictionary<string, UnitLoadData> unitData;
        //weapons loaded in form json
        public Dictionary<int, Weapon> weaponData;

        void Awake()
        {
            unitData = LoadSystem.LoadUnits();
            weaponData = LoadSystem.LoadWeapons();
        }

        public void Load(UnitController unitController)
        {
            try
            {
                UnitLoadData unit;
                if (!unitData.TryGetValue(unitController.name, out unit))
                {
                    if (!unitData.TryGetValue(EnvironmentUtil.DEFAULT_UNIT, out unit)) throw new Exception();
                }

                Weapon weapon;
                if (!weaponData.TryGetValue(unit.weapon, out weapon)) throw new Exception();

                unitController.unit = Unit.CreateUnit(unitController, unit, weapon);
                
                if (Add(unitController)) print("Successfully loaded: " + unitController.name);
                else throw new Exception();
            }
            catch (Exception e)
            {
                print("Failed to load: " + unitController.name);
                Destroy(unitController.gameObject);
            }
        }

        bool Add(IUnitController controller)
        {
            if (controller is ITimeSensitive timeSensitive) recordables.Add(timeSensitive);
            return units.Add(controller);
        }

        bool Remove(IUnitController controller)
        {
            if (controller is ITimeSensitive timeSensitive) recordables.Remove(timeSensitive);
            return units.Remove(controller);
        }
        
        public static Brawl InitBrawl()
        {
            var brawlObject = new GameObject();
            brawlObject.name = "brawl_" + brawlObject.GetInstanceID().ToString().Replace("-", "");
            //setting up brawl behaviour
            return brawlObject.AddComponent<Brawl>();
        }
    }
}
