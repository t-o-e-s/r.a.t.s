using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    public bool bridgeUp;
    public GameObject bridge;

    private void Update()
    {
        if(bridge.transform.position.y >= -0.8f)
        {
            bridgeUp = true;
        }
    }


    public void RewindBridge()
    {
        if (!bridgeUp)
        {
            bridge.transform.position += new Vector3(0, 1 * Time.deltaTime, 0);
        }
    }
}
