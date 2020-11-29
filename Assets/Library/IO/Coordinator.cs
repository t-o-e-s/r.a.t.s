﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinator
{    
    public static GameObject[] GetAdjacent(GameObject tile)
    {

        //TODO ensure scalability when using tiles that are not 1:1:1
        float offset = tile.transform.localScale.x >= tile.transform.localScale.z ?
            tile.transform.localScale.x :
            tile.transform.localScale.z;

        //offset = offset + (offset / 2);

        Vector3 origin = tile.transform.position;

        Vector3[] testPoints =
        {
            new Vector3(origin.x + offset, origin.y, origin.z),
            new Vector3(origin.x - offset, origin.y, origin.z),
            new Vector3(origin.x, origin.y, origin.z + offset),
            new Vector3(origin.x, origin.y, origin.z - offset)
        };

        HashSet<GameObject> output = new HashSet<GameObject>();

        foreach(Vector3 v in testPoints)
        {
            Collider[] colls = Physics.OverlapSphere(
                v,
                (offset / 4),
                ~0,
                QueryTriggerInteraction.Collide);

            if (colls.Length == 1)
            {
                output.Add(colls[0].gameObject);
            }
            else if (colls.Length > 1)
            {
                bool add = true;

                foreach(Collider c in colls)
                {
                    //if any of the colliders are not the environment then don't add
                    if (c.gameObject.layer == LayerMask.NameToLayer("environment"))
                    {
                        continue;
                    }
                    else if (c.CompareTag("movement_tile"))
                    {
                        output.Add(c.gameObject);
                        continue;
                    }

                    add = false;
                }
            }
            else
            {
                //DO NOTHING
            }
        }

        Debug.Log("Found " + output.Count +" suitable tiles near " + tile.name);

        //Converting to array for output
        GameObject[] outArr = new GameObject[output.Count];
        output.CopyTo(outArr);
        return outArr;
    }
}
