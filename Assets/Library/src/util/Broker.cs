using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Library.src.combat;
using Library.src.combat.Weapon;
using Library.src.elements;
using Library.src.units;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Library.src.util
{
    public class Broker : MonoBehaviour
    {
        public float stoppingDistance = 0.3f;

        HashSet<IResolvable> resolvables = new HashSet<IResolvable>();
    
        HashSet<IResolvable> combats = new HashSet<IResolvable>();
        IResolvable[][] combatBatches;

        //HashSets for tracking active units
        public HashSet<IUnitController> units = new HashSet<IUnitController>();
        public HashSet<IUnitController> playerUnits = new HashSet<IUnitController>();
        public HashSet<IUnitController> aiUnits = new HashSet<IUnitController>();

        Stopwatch watch;

        [SerializeField]
        bool isTest = true;
        [SerializeField]
        int ticksPerSecond = 3;
        int frameCount = 1;
    
    
        [SerializeField] int performanceDivider = 1;

        void Awake()
        {
            IUnitController unit;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("player_unit"))
            {
                if (go.TryGetComponent(out unit)) playerUnits.Add(unit);
            }
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("enemy_unit"))
            {
                if (go.TryGetComponent(out unit)) playerUnits.Add(unit);
            }
        
            Debug.Log(playerUnits.Count);
            if (isTest) TestSetUp();
        }

        void FixedUpdate()
        {
            if (frameCount == 60)
            {
                frameCount = 1;
                StartCoroutine(Resolve());
            }
        }

        IEnumerator Resolve()
        {
            foreach (var batch in combatBatches)
            {
                foreach (var combat in batch)
                {
                    combat.Resolve();
                }

                yield return null;
            }
        }

        public void LoadAs(UnitController unitController)
        {
            if (isTest)
            {
                //instantiates a new unit object and loads it onto the controller
                var unit = new Unit();
                unit.name = unitController.name;
                unit.health = 100f;
                unit.speed = unitController.GetSpeed();
                unit.weapon = Arsenal.Fists();
                unit.statuses = new Status[0];
                unitController.unit = unit;
            }
        }
    
        public bool Add(IResolvable resolvable)
        {
            if (resolvable is ICombat) combats.Add(resolvable);
            var success = resolvables.Add(resolvable);
            if (success) UpdateBatches();
            return success;
        }

        public bool Remove(IResolvable resolvable)
        {
            if (resolvable is ICombat) combats.Remove(resolvable);
            var success = resolvables.Remove(resolvable);
            if (success) UpdateBatches();
            return success;
        }

        public bool Add(IUnitController controller)
        {
            return units.Add(controller);
        }

        public bool Remove(IUnitController controller)
        {
            return units.Remove(controller);
        }

        void UpdateBatches()
        {
            combatBatches = new IResolvable[performanceDivider][];
            int arr = 0;
            int i = combats.Count % performanceDivider;
            int arrSize = combats.Count / performanceDivider;
            arrSize = i == 0 ? arrSize : arrSize + 1;
            i = 0; 
        
            foreach (var c in combats)
            {
                if (i == arrSize)
                {
                    i = 0;
                    arr++;
                }

                //Debug.Log(String.Format("Populated combat batch [{0}][{1}]", arr, i));
                combatBatches[arr++][i++] = c;
            }
        }
    
        // This method should change depending on what needs to be implemented as the project evolves
        void TestSetUp()
        {
        
        }

        public bool IsTest()
        {
            return isTest;
        }
    
    }
}
