using System.Collections.Generic;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;
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
        
        public static Brawl InitBrawl()
        {
            var brawlObject = new GameObject();
            brawlObject.name = "brawl_" + brawlObject.GetInstanceID().ToString().Replace("-", "");
            //setting up brawl behaviour
            return brawlObject.AddComponent<Brawl>();
        }

        public void Load(UnitController unitController)
        {
            if (IsTest())
            {
                //instantiates a new unit object and loads it onto the controller
                unitController.unit = Unit.CreateUnit(
                    unitController.name,
                    unitController,
                    100f,
                    unitController.agent.speed,
                    new Status[0],
                    null,
                    Arsenal.Fists());
            }

            if (Add(unitController)) print("Succesfully loaded: " + unitController.name);
            else print("Failed to load: " + unitController.name);
        }
        
        public bool IsTest()
        {
            return Application.isEditor;
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
    }
}
