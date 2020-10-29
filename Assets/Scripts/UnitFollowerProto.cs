using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFollowerProto : MonoBehaviour
{
    public GameObject leader;
    Vector3 leaderPos;
    float speed;

    void Start()
    {
        speed = 9f;
    }

    // Update is called once per frame
    void Update()
    {
        leaderPos = leader.transform.position;

        this.gameObject.transform.position = Vector3.MoveTowards(transform.position, leaderPos, Time.deltaTime * speed);
    }
}
