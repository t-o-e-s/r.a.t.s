using System.Collections.Generic;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;
using Library.src.time;
using Library.src.units;
using UnityEngine;

namespace Library.src.util
{
    public class Broker : MonoBehaviour
    {
        //HashSets for tracking active units
        public readonly HashSet<IUnitController> units = new HashSet<IUnitController>();
        public readonly HashSet<IUnitController> playerUnits = new HashSet<IUnitController>();
        public readonly HashSet<IUnitController> aiUnits = new HashSet<IUnitController>();
        //tracking time sensitive objects
        public readonly HashSet<TimeSensitive> recordables = new HashSet<TimeSensitive>();

        [SerializeField]
        bool isTest = true;
        int frameCount = 1;

        void Awake()
        {
            IUnitController unit;
            foreach (var go in GameObject.FindGameObjectsWithTag("player_unit"))
            {
                if (go.TryGetComponent(out unit)) playerUnits.Add(unit);
            }
            foreach (var go in GameObject.FindGameObjectsWithTag("enemy_unit"))
            {
                if (go.TryGetComponent(out unit)) playerUnits.Add(unit);
            }
        
            if (isTest) TestSetUp();
        }

        public void LoadAs(UnitController unitController)
        {
            if (isTest)
            {
                //instantiates a new unit object and loads it onto the controller
                unitController.unit = new Unit(
                    unitController.name,
                    unitController,
                    100f,
                    unitController.GetSpeed(),
                    new Status[0],
                    null,
                    Arsenal.Fists());
            }
        }
        
        public bool Add(IUnitController controller)
        {
            
            if (controller is TimeSensitive timeSensitive) recordables.Add(timeSensitive);
            return units.Add(controller);
        }

        public bool Remove(IUnitController controller)
        {
            if (controller is TimeSensitive timeSensitive) recordables.Remove(timeSensitive);
            return units.Remove(controller);
        }

        // This method should change depending on what needs to be implemented as the project evolves
        void TestSetUp()
        {
        
        }

        public static Brawl InitBrawl()
        {
            var brawlObject = new GameObject();
            brawlObject.name = "brawl_" + brawlObject.GetInstanceID().ToString().Replace("-", "");
            //setting up brawl behaviour
            return brawlObject.AddComponent<Brawl>();
        }

        public bool IsTest()
        {
            return isTest;
        }

    }
}
