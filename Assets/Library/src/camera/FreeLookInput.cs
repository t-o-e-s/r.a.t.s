using Cinemachine;
using UnityEngine;

namespace Library.src.camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookInput : MonoBehaviour
    {
        CinemachineFreeLook freeLookCamera;

        string xAxisName = "Mouse X";
        string yAxisName = "Mouse Y";

        void Start()
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }

        void Update()
        {
            //Need to figure out how to lock camera when we need to (eg. when dialogue is playing)
            if (Input.GetMouseButton(2))
            {
                freeLookCamera.m_XAxis.m_InputAxisValue = Input.GetAxis(xAxisName);
                freeLookCamera.m_YAxis.m_InputAxisValue = Input.GetAxis(yAxisName);
            }
            else
            {
                freeLookCamera.m_XAxis.m_InputAxisValue = 0;
                freeLookCamera.m_YAxis.m_InputAxisValue = 0;
            }
        }
    }
}
