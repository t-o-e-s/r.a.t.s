using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.management.units;
using Library.src.units.control;


namespace Library.src.units
{
    public class UiHealth : MonoBehaviour
    {
        public UnitController unitCon;
       

        private float maxHp = 100.0f;
        private float currentHp;
        public Canvas can;
        public Camera cam;
     
        void Update()
        {
           currentHp = unitCon.unit.health;
           transform.localScale = new Vector3(currentHp / maxHp, 1.0f, 1.0f);
           //can.transform.LookAt(can.transform.position);
           can.transform.LookAt(cam.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

        }
    }
}
