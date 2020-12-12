using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Library.Combat;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Broker : MonoBehaviour
{
    public readonly float combatDistance = 3f;

    HashSet<IResolvable> resolvables = new HashSet<IResolvable>();
    HashSet<ICombat> combats = new HashSet<ICombat>();

    Object[] combatBatches;

    Stopwatch watch;

    [SerializeField]
    int ticksPerSecond = 3;

    [SerializeField] int performanceDivider = 1;

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
        foreach (Object batch in combatBatches)
        {
            r.Resolve();
            yield return null;
        }
    }

    public bool Add(IResolvable resolvable)
    {
        bool success = resolvables.Add(resolvable);

        return success;
    }

    public bool Remove(IResolvable resolvable)
    {
        return resolvables.Remove(resolvable);
    }

    void UpdateBatches()
    {
        var batchSize = combats.Count;
        batchSize = batchSize / performanceDivider;
        combatBatches = new Object[batchSize];
        //TODO populate batches
    }
}
