using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RayCastMovement : MonoBehaviour
{

    GameObject[] units;
    void Awake()
    {
        units = GameObject.FindGameObjectsWithTag("Unit");              
    }

    // Update is called once per frame
    void Update()
    {
       // if (Input.GetMouseButtonDown(0)) 
            //HandleMovement();

        foreach (GameObject unit in units)
        {
  
            if (unit.GetComponent<UnitSelection>().unitSelected == true && Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
                {
                    if (hit.collider.CompareTag("movement_tile"))
                    {
                        Vector3 v = new Vector3(
                            hit.collider.gameObject.transform.position.x, 0,
                            hit.collider.gameObject.transform.position.z);

                        unit.GetComponent<NavMeshAgent>().SetDestination(v);
                    }

                }
            }

        }
    }

   /* void HandleMovement()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100f))
        {
            if (hit.collider.CompareTag("movement_tile"))
            {
                Vector3 v = new Vector3(
                    hit.collider.gameObject.transform.position.x, 0, 
                    hit.collider.gameObject.transform.position.z);

                unit.GetComponent<NavMeshAgent>().SetDestination(v);
            }

        }
    }*/
}
