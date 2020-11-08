using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatResolver : MonoBehaviour, IResolvable
{
    HashSet<UnitController> ratUnits = new HashSet<UnitController>();
    HashSet<UnitController> eiUnits = new HashSet<UnitController>();    

    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resolve()
    {
        StartCoroutine("Run");
    }

    //might need the why explained in person as to why this is a coroutine, but look it up in the API also :)
    IEnumerator Run()
    {
        if (ratUnits.Count == 0 || eiUnits.Count == 0) Destroy(this);

        //creates unitGroup for rats then waits
        UnitGroup rats = new UnitGroup(ratUnits);
        yield return null;

        //creates unitGroup for EI then waits
        UnitGroup ei = new UnitGroup(eiUnits);
        yield return null;

        //calculate damage to rats & ei
        float damageToRats = CalculateDamage(ei, rats);
        float damageToEi = CalculateDamage(rats, ei);

        //applying damage
        rats.TakeDamage(damageToRats);
        ei.TakeDamage(damageToEi);

        //deleting objects as not to clog up memory with shite
        rats = null;
        ei = null;
    }


    public CombatResolver AddRats(UnitController rat)
    {
        ratUnits.Add(rat);
        return this;
    }

    public CombatResolver AddEnemy(UnitController enemy)
    {
        eiUnits.Add(enemy);
        return this;
    }

    //this probably will change but it's a start for the prototype
    float CalculateDamage(UnitGroup attacker, UnitGroup defender) 
    {
        return attacker.attackPower - defender.defensePower;
    }

    private void OnDestroy()
    {
        //implement end of combat
    }
}
