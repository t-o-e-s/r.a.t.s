using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Library.Combat;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class Broker : MonoBehaviour
{
    public float combatDistance = 3f;

    HashSet<IResolvable> resolvables = new HashSet<IResolvable>();
    
    HashSet<ICombat> combats = new HashSet<ICombat>();
    ICombat[][] combatBatches;

    //HashSets for tracking active units
    public HashSet<UnitController> playerUnits = new HashSet<UnitController>();
    public HashSet<UnitController> aiUnits = new HashSet<UnitController>();

    Stopwatch watch;

    [SerializeField]
    bool isTest = true;
    [SerializeField]
    int ticksPerSecond = 3;
    int frameCount = 1;
    
    
    [SerializeField] int performanceDivider = 1;

    void Awake()
    {
        watch = new Stopwatch();
        watch.Start();
        InvokeRepeating("RunResolution", 0f, 1f / ticksPerSecond);

        UnitController unitCon;
        //populating both lists        
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("player_unit"))
        {
            
            if (go.TryGetComponent(out unitCon)) playerUnits.Add(unitCon);

           
        }

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("enemy_unit"))
        {
        
            if (go.TryGetComponent(out unitCon)) playerUnits.Add(unitCon);


        }

        Save.SaveRoster(this);
    }

    //calling the CheckUnits method to see if playerUnits and aiUnits are alive or dead 
    private void FixedUpdate()
    {
        CheckUnits();
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

    //checks the health of the units in the list, if >= to 0 then removes them from the list
    void CheckUnits()
    {
        /*foreach (GameObject player_unit in playerUnits)
        {
            if(player_unit.GetComponent<MeleeController>().health >= 0)
            {
                playerUnits.Remove(player_unit);
            }
        }


        foreach (GameObject aiUnits in aiUnits)
        {
            //TODO not sure where or how aiUnits health is managed 
            //code to go in here to remove them from list 
        }*/
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
