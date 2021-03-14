using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Library.src.units;
using Library.src.units.control;
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

        bool end = false;


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
            if (watch.ElapsedMilliseconds >= EnvironmentUtil.BRAWL_RESOLUTION_INTERVAL)
            {
                StartCoroutine(Fight());
                watch.Restart();
            }
            if (end) End();
        }

        void OnTriggerEnter(Collider other)
        {
            if (playerUnits.Count + aiUnits.Count >= EnvironmentUtil.BRAWL_MAX_PARTICIPANT) return;
            if (!other.gameObject.TryGetComponent(out UnitController controller)) return;
                
            switch (other.tag)
            {
                case (EnvironmentUtil.TAG_PLAYER):
                    playerUnits.Add(controller);
                    if (controller.GetTarget() == null) GiveTarget(controller);
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
            //TODO method prioritises player units - this will need sorting
            var unitCollections = new []{playerUnits, aiUnits};
            //damage resolution is split over two frames, this can be increased if required
            foreach (var collection in unitCollections)
            {
                foreach (var uC in collection)
                {
                    uC.DealDamage();
                    if (uC.GetTarget() is null) {} //TODO assign new target to the unit.
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
            bool success = false;
            
            if (unitController.Equals(null)) return false;
            
            if (unitController.CompareTag(EnvironmentUtil.TAG_PLAYER))
            {
                success = playerUnits.Add(unitController);
            }
            if (unitController.CompareTag(EnvironmentUtil.TAG_AI))
            {
                success = aiUnits.Add(unitController);
            }

            if (success) UpdatePosition();
            return success;
        }  
        
        public bool RemoveUnit(UnitController unitController)
        {
            bool success = false;
            
            if (unitController.Equals(null)) return false;
            
            if (unitController.CompareTag(EnvironmentUtil.TAG_PLAYER))
            {
                success = playerUnits.Remove(unitController);
            }
            if (unitController.CompareTag(EnvironmentUtil.TAG_AI))
            {
                success = aiUnits.Remove(unitController);
            }

            if (success && FightShouldEnd())
            {
                end = true;
                return success;
            }
            if (success) UpdatePosition();
            return false;
        }
        
        void UpdatePosition()
        {
            var participants = AllUnits();
            

            if (participants.Count() == 1)
            {
                transform.position = participants[0].transform.position;
                return;
            }

            var centroid = Vector3.zero;
            foreach (var uC in participants)
            {
                centroid += uC.transform.position;
            }
            transform.position = centroid / participants.Length;
        }

        UnitController[] AllUnits()
        {
            var participants = new UnitController[playerUnits.Count + aiUnits.Count];
            playerUnits.CopyTo(participants, 0);
            aiUnits.CopyTo(participants, playerUnits.Count);
            return participants;
        }

        bool FightShouldEnd()
        {
            return (playerUnits.Count == 0 && aiUnits.Count >= 0)
                   || (playerUnits.Count >= 0 && aiUnits.Count == 0);
        }

        void End()
        {
            print("[Update] - " + gameObject.name + " brawl ending.");
            Destroy(gameObject);
        }

    }
}