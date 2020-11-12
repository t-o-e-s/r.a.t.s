using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMech : MonoBehaviour
{
    public TimeHandler timeH;
    BridgeHandler bridgeH;



    private void OnTriggerStay(Collider other)
    {
        bridgeH = other.gameObject.GetComponent<BridgeHandler>();

        if (timeH.isRewinding == true)
        {
            if (other.gameObject.tag == "timeTile" && bridgeH.bridgeUp == false)
            {
                bridgeH.RewindBridge();
            }
        }
    }
}
