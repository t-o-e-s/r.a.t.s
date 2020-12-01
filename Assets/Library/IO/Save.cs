using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save 
{

   
    private void SaveRoster(Broker broker)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/availableRoster.dat");

        Roster roster = new Roster();

            //TODO Currently only saving alive units to activeUnits Roster, I still need to save the unused Warband here as well 
            foreach (GameObject playerUnit in broker.playerUnits)
            {
            roster.activeUnits.Add(playerUnit);
            }
                             
        bf.Serialize(file, roster);
        file.Close();
    }  
       
    public void LoadRoster()
    {
        if (File.Exists(Application.persistentDataPath + "/availableRoster.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/availableRoster.dat", FileMode.Open);
            Roster roster = (Roster)bf.Deserialize(file);
            file.Close();

            //We can set the data from the loaded roster here, perhaps send it back to the broker to load the new active units, or we can use the load elsewhere 
        }
    }


}
