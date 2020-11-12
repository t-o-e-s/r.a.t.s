using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TimeHandler : MonoBehaviour
{
    public List<Vector3> unitPositions;
    private GameObject unit;
    public bool isRewinding;
    private NavMeshAgent agent;
    private Vector3 destination;
    UnitController unitCont;
    private void Awake()
    {
        unit = gameObject;
        agent = GetComponent<NavMeshAgent>();
        unitCont = GetComponent<UnitController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isRewinding = true;
        }
        else
        {
            isRewinding = false;
        }
    }

    private void FixedUpdate()
    {
        if (!isRewinding)
        {
            unitPositions.Add(unit.transform.position);
        }
        else
        {
            destination = (Vector3)unitPositions[unitPositions.Count - 1];
            unitPositions.RemoveAt(unitPositions.Count - 1);
            unitCont.Move(target: destination);
        }
    }
}
