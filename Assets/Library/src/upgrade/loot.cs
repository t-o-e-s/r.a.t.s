using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loot 
{
    

    public static void GetLoot()
    {
        int lootClass;

        System.Random rnd = new System.Random();
        lootClass = rnd.Next(1, 6);
        Debug.Log(lootClass);
    }
}
