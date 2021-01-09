using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Library.src.units;
using Library.src.util;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Library.src.combat
{
    public class Brawl : MonoBehaviour
    {
        List<UnitController> playerUnits = new List<UnitController>();
        List<UnitController> aiUnits = new List<UnitController>();

        CapsuleCollider col;

        Stopwatch watch = new Stopwatch();

        int participants = 0;
        
        
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
            if (participants >= EnvironmentUtil.BRAWL_MAX_PARTICIPANT) return;
            if (!other.gameObject.TryGetComponent(out UnitController controller)) return;
                
            switch (other.tag)
            {
                case (EnvironmentUtil.TAG_PLAYER):
                    playerUnits.Add(controller);
                    GiveTarget(controller);
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
            if (participants >= EnvironmentUtil.BRAWL_MAX_PARTICIPANT) return;
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
                ? aiUnits
                : playerUnits;
            
            //TODO create a threat based system that decides the way in which units are selected - for now it's rando
            unitController.SetTarget(potentialTargets[Random.Range(0, (potentialTargets.Count - 1))].unit);
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
        void UpdatePosition()
        {
            //TODO calculate a median position for the brawl object dependant on the location of participating units
        }
        
    }
}