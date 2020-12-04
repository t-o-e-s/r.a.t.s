using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save
{
       
    public static void SaveRoster(Broker broker)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/availableRoster.dat");

        Roster roster = new Roster(broker.playerUnits, broker.aiUnits);

            //TODO Currently only saving alive units to activeUnits Roster, I still need to save the unused Warband here as well      
            //TODO code to compare the new save with the roster of the previous save to see which rats have died
       
                             
        bf.Serialize(file, roster);
        file.Close();

        Debug.Log(Application.persistentDataPath);
    }  
       
    public static void LoadRoster(Broker broker)
    {
        if (File.Exists(Application.persistentDataPath + "/availableRoster.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/availableRoster.dat", FileMode.Open);
            Roster roster = (Roster) bf.Deserialize(file);
            file.Close();

            broker.playerUnits = roster.activeUnits;
            //We can set the data from the loaded roster here, perhaps send it back to the broker to load the new active units, or we can use the load elsewhere 
        }
    }
    
}
