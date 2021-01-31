using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Loot 
{
    public static int LOOT_CLASS;
    public static void GetLoot()
    {
       

        System.Random rnd = new System.Random();
        LOOT_CLASS = rnd.Next(1, 6);
        Debug.Log("lootclass =" + LOOT_CLASS);  
    }
}
