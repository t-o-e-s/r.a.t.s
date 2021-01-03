﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class FreeLookInput : MonoBehaviour
{
    CinemachineFreeLook freeLookCamera;

    string XAxisName = "Mouse X";
    string YAxisName = "Mouse Y";

    //Start is called before the first frame update
    void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
    }

    //Update is called once per frame
    void Update()
    {
        //Need to figure out how to lock camera when we need to (eg. when dialogue is playing)
        if (Input.GetMouseButton(2))
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = Input.GetAxis(XAxisName);
            freeLookCamera.m_YAxis.m_InputAxisValue = Input.GetAxis(YAxisName);
        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisValue = 0;
            freeLookCamera.m_YAxis.m_InputAxisValue = 0;
        }
    }
}
