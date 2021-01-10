using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Library.src.units;
using Library.src.util;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Library.src.combat
{
    public class Brawl : MonoBehaviour
    {
        //collections for units
        readonly HashSet<UnitController> playerUnits = new HashSet<UnitController>();
        readonly HashSet<UnitController> aiUnits = new HashSet<UnitController>();

        //used in timing combat resolution - this might fit on the broker, it might also work on a frame count we shall see
        readonly Stopwatch watch = new Stopwatch();
        
        //collider used to demarcate the brawl
        CapsuleCollider col;


        /*
         * =====================
         *  Unity Functions
         * =====================
         */
        void Awake()
        {
            col = gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            if (!(col is null))
            {
                col.radius = EnvironmentUtil.BRAWL_DISTANCE;
                col.height = EnvironmentUtil.BRAWL_HEIGHT;
                col.isTrigger = true;
            }
            else
            {
                Debug.LogError("[Brawl] - Could not be instantiated as CapsuleCollider is null");
                Destroy(this);
            }
            
            watch.Start();
        }

        void Update()
        {
            if (watch.ElapsedMilliseconds == EnvironmentUtil.BRAWL_RESOLUTION_INTERVAL)
            {
                StartCoroutine(Fight());
                watch.Restart();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (playerUnits.Count + aiUnits.Count >= EnvironmentUtil.BRAWL_MAX_PARTICIPANT) return;
            if (!other.gameObject.TryGetComponent(out UnitController controller)) return;
                
            switch (other.tag)
            {
                case (EnvironmentUtil.TAG_PLAYER):
                    playerUnits.Add(controller);
                    if (controller.GetTarget().Equals(null)) GiveTarget(controller);
                    break;
                case (EnvironmentUtil.TAG_AI):
                    aiUnits.Add(controller);
                    GiveTarget(controller);
                    break;
            }
            
            UpdatePosition();
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out UnitController controller)) return;
                
            switch (other.tag)
            {
                case (EnvironmentUtil.TAG_PLAYER):
                    playerUnits.Remove(controller);
                    ClearTarget(controller);
                    break;
                case (EnvironmentUtil.TAG_AI):
                    aiUnits.Remove(controller);
                    ClearTarget(controller);
                    break;
            }
            
            UpdatePosition();
        }


        /*
         * =====================
         *  Fight Functions
         * =====================
         */
        IEnumerator Fight()
        {
            var unitCollections = new []{playerUnits, aiUnits};
            
            //damage resolution is split over two frames, this can be increased if required
            foreach (var collection in unitCollections)
            {
                foreach (var uC in collection)
                {
                    uC.DealDamage();                                                                                                                                                                                                
                }
                yield return null;
            }
        }

        /*
         * =====================
         *  Unit Related Functions
         * =====================
         */
        void GiveTarget(UnitController unitController)
        {
            var potentialTargets = unitController.CompareTag(EnvironmentUtil.TAG_PLAYER)
                ? aiUnits.ToArray()
                : playerUnits.ToArray();
            
            //TODO create a threat based system that decides the way in which units are selected - for now it's random
            unitController.SetTarget(potentialTargets[Random.Range(0, potentialTargets.Length - 1)].unit);
        }

        void ClearTarget(UnitController unitController)
        {
            unitController.SetTarget(null);
        }
        
        /*
         * =====================
         *  Utility Functions
         * =====================
         */
        public bool AddUnit(UnitController unitController)
        {
            if (unitController.Equals(null)) return false;
            
            if (unitController.CompareTag(EnvironmentUtil.TAG_PLAYER))
            {
                return playerUnits.Add(unitController);
            }
            if (unitController.CompareTag(EnvironmentUtil.TAG_AI))
            {
                return aiUnits.Add(unitController);
            }

            return false;
        }  
        
        public bool RemoveUnit(UnitController unitController)
        {
            if (unitController.Equals(null)) return false;
            
            if (unitController.CompareTag(EnvironmentUtil.TAG_PLAYER))
            {
                return playerUnits.Remove(unitController);
            }
            if (unitController.CompareTag(EnvironmentUtil.TAG_AI))
            {
                return aiUnits.Remove(unitController);
            }
            
            return false;
        }
        
        void UpdatePosition()
        {
            //TODO calculate a median position for the brawl object dependant on the location of participating units
        }

    }
}