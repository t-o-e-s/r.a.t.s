using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    public readonly GameObject gameObject;

    //location
    public readonly Vector3 position;
    public readonly Quaternion rotation;

    //health & combat
    public readonly float health;

    //other
    public readonly Status[] statuses;

    public readonly bool inCombat;

    public State(GameObject gameObject, Vector3 position, Quaternion rotation, float health, Status[] statuses, bool inCombat)
    {
        this.position = position;
        this.rotation = rotation;
        this.health = health;
        this.statuses = statuses;
        this.inCombat = inCombat;
    }
}
