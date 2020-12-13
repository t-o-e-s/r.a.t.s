using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    public bool bridgeUp;
    public GameObject bridge;

    private void Update()
    {
        //default position of bridge so it is crossable
        if(bridge.transform.position.y >= -0.8f)
        {
            bridgeUp = true;
        }
    }

    //simple pulls teh bridge upwards by adding to its transform 
    public void RewindBridge()
    {
        if (!bridgeUp)
        {
            bridge.transform.position += new Vector3(0, 1 * Time.deltaTime, 0);
        }
    }
}
