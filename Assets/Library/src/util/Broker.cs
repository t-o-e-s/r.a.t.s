using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Library.Combat;
using Library.Units;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class Broker : MonoBehaviour
{
    public float stoppingDistance = 0.3f;

    HashSet<IResolvable> resolvables = new HashSet<IResolvable>();
    
    HashSet<ICombat> combats = new HashSet<ICombat>();
    ICombat[][] combatBatches;

    //HashSets for tracking active units
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
                combat.ResolveDamage();
            }

            yield return null;
        }
    }
    
    public bool Add(IResolvable resolvable)
    {
        if (resolvable is ICombat c) combats.Add(c);
        var success = resolvables.Add(resolvable);
        if (success) UpdateBatches();
        return success;
    }

    public bool Remove(IResolvable resolvable)
    {
        if (resolvable is ICombat c) combats.Remove(c);
        var success = resolvables.Remove(resolvable);
        if (success) UpdateBatches();
        return success;
    }

    void UpdateBatches()
    {
        combatBatches = new ICombat[performanceDivider][];
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

            Debug.Log(String.Format("Populated combat batch [{0}][{1}]", arr, i));
            combatBatches[arr++][i++] = c;
        }
    }


    // This method should change depending on what needs to be implemented as the project evolves
    void TestSetUp()
    {
        
    }
    
}
