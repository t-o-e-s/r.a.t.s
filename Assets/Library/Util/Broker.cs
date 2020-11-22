using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Broker : MonoBehaviour
{
    public readonly float combatDistance = 3f;

    HashSet<IResolvable> resolvables = new HashSet<IResolvable>();

    Stopwatch watch;

    [SerializeField]
    int ticksPerSecond = 3;    

    void Awake()
    {
        watch = new Stopwatch();
        watch.Start();
        InvokeRepeating("RunResolution", 0f, 1f / ticksPerSecond);
    }

    //resolving
    void RunResolution()
    {
        StartCoroutine("Resolve");
    }

    IEnumerator Resolve()
    {
        //everything that can resolve will do so on seperate frames to avoid stutters
        foreach (IResolvable r in resolvables)
        {
            r.Resolve();
            yield return null;
        }
    }

    public bool Add(IResolvable resolvable)
    {
        return resolvables.Add(resolvable);
    }

    public bool Remove(IResolvable resolvable)
    {
        return resolvables.Remove(resolvable);
    }
}
