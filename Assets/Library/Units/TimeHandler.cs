using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHandler : MonoBehaviour
{
    public List<Vector3> unitPositions;
    private GameObject unit;
    public bool isRewinding;

    private void Awake()
    {
        unit = gameObject;
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
            unit.transform.position = (Vector3)unitPositions[unitPositions.Count - 1];
            unitPositions.RemoveAt(unitPositions.Count - 1);
        }
    }
}
