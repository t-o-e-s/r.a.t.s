using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Broker : MonoBehaviour
{
    public readonly float combatDistance = 3f;

    HashSet<IResolvable> resolvables = new HashSet<IResolvable>();

    //HashSets for tracking active units
    public HashSet<UnitController> playerUnits = new HashSet<UnitController>();
    public HashSet<UnitController> aiUnits = new HashSet<UnitController>();

    Stopwatch watch;

    [SerializeField]
    int ticksPerSecond = 3;

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
    }

    //resolving
    void RunResolution()
    {
        StartCoroutine("Resolve");
    }

    IEnumerator Resolve()
    {
        //everything that can resolve will do so on seperate frames to avoid stutters
        foreach (IResolvable r in resolvables)
        {
            r.Resolve();
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
        return resolvables.Add(resolvable);
    }

    public bool Remove(IResolvable resolvable)
    {
        return resolvables.Remove(resolvable);
    }

}
