using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    private Statuses[] statuses;

    private GameObject gameObject;

    private int interval;

    protected State(GameObject gameObject, int interval)
    {
        this.gameObject = gameObject;
        this.interval = interval;
    }
}
