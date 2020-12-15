using UnityEngine;

namespace Library.src.units
{
    public class TimeMech : MonoBehaviour
    {
        //a time handler, doesn't matter which, its just for reading if isRewinding == true
        public TimeHandler timeH;

        //getting the bridgeHandler attached to a 'bridge rewind' point, designated in orange on map 
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
}
