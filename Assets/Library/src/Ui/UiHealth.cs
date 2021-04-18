using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Library.src.management.units;
using Library.src.units.control;
using UnityEngine.UI;


namespace Library.src.units
{
    public class UiHealth : MonoBehaviour
    {
        public UnitController unitCon;
        public Image hp;
        public Image flag;
      
        private float maxHp = 100.0f;
        private float currentHp;
        Canvas can;
        Camera cam;

        private void Awake()
        {
            can = GetComponent<Canvas>();
            cam = Camera.main;
            flag.enabled = false;
        }

        void Update()
        {
           currentHp = unitCon.unit.health;
           hp.transform.localScale = new Vector3(currentHp / maxHp, 1.0f, 1.0f);
           can.transform.LookAt(cam.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

           if (unitCon.flagEnabled == true)
            {
                flag.enabled = true;
            }
           else if (unitCon.flagEnabled == false)
            {
                flag.enabled = false;
            }
        }


    }
}
