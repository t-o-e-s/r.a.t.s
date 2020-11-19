using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatResolver : MonoBehaviour, IResolvable
{
    //code will not actual reference these, or atleast it shouldn't
    HashSet<UnitController> attacking = new HashSet<UnitController>();
    HashSet<UnitController> defending = new HashSet<UnitController>();
    //instead it will reference these bad boys
    HashSet<UnitController> player;
    HashSet<UnitController> ai;

    HashSet<UnitController> incoming = new HashSet<UnitController>();

    //this marks the original defending units, if he is dead then defenders may no longer get defense bonus???
    UnitController defender;
   
    bool run = false;

    float combatDistance;

    void Awake()
    {
        combatDistance = Camera.main.GetComponent<Broker>().combatDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(UnitController attacker, UnitController defender)
    {
        run = true;

        //setting both player and ai sets as references to attack and defense
        player = attacker.playerUnit ?
            attacking :
            defending;

        ai = attacker.playerUnit ?
             defending :
             attacking;

        //add defender to set
        defending.Add(defender);

        //if the attacker is near the defender then place him in the attacking
        if (InProximity(attacker.transform, defender.transform))
        {
            attacking.Add(attacker);
        }
        else
        {
            incoming.Add(defender);
        }
    }

    public void Resolve()
    {
        StartCoroutine("Run");
    }

    //might need the why explained in person as to why this is a coroutine, but look it up in the API also :)
    IEnumerator Run()
    {
        if (player.Count == 0 || ai.Count == 0) Destroy(this);

        //creates unitGroup for rats then waits
        UnitGroup rats = new UnitGroup(player);
        yield return null;

        //creates unitGroup for EI then waits
        UnitGroup ei = new UnitGroup(ai);
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


    public CombatResolver Add(UnitController unit)
    {
       if (InProximity(unit.transform, defender.transform))
        {
            if (unit.playerUnit)
            {
                player.Add(unit);
            }        
            else
            {
                ai.Add(unit);
            }
        }
       else
        {
            incoming.Add(unit);
        }
        return this;
    }

    public bool Arrived(UnitController unit, UnitController target)
    {
        if (InProximity(unit.transform, target.transform))
        {
            incoming.Remove(unit);
            if (unit.playerUnit)
            {
                player.Add(unit);
            }
            else
            {
                ai.Add(unit);
            }

            return true;
        }

        return false;
    }

    //this probably will change but it's a start for the prototype
    float CalculateDamage(UnitGroup attacker, UnitGroup defender) 
    {
        return attacker.attackPower - defender.defensePower;
    }

    bool InProximity(Transform attacker, Transform defender) 
    {
        return Vector3.Distance(attacker.position, defender.position) <= combatDistance;
    }

    private void OnDestroy()
    {
        //implement end of combat
    }
}
