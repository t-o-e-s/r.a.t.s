using System.Collections;
using System.Collections.Generic;
using Library.src.time;
using Library.src.units;
using Library.src.util;
using UnityEngine;

public class IOHandler : MonoBehaviour
{
    Broker broker;
    
    Camera mainCam;

    //buffers
    UnitController unitBuffer;
    TimeSensitive timedBuffer;

    Time time;

    bool aUnitSelected;

    int lootTotal;

    [SerializeField] int framerate = 60;
    
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
        TryGetComponent(out broker);
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = framerate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != framerate) Application.targetFrameRate = framerate;
        
        if (Input.GetMouseButtonDown(0)) Cast();
        
        //reverse time
        if (Input.GetKeyDown(KeyCode.R)) HandleRewind(timedBuffer);

        //foreach (var timed in broker.recordables) timed.Record();
        
        
    }

    void Cast()
    {       
        RaycastHit hit;
       
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 150f))
        {
            switch (hit.collider.tag)      
            {
                case "movement_tile":
                    HandleMovement(hit.point);
                    break;

                case EnvironmentUtil.TAG_PLAYER:
                    HandleSelection(hit.collider.gameObject);
                    break;

                case EnvironmentUtil.TAG_AI:
                 
                    HandleAttack(hit.collider.gameObject);
                    break;

                case EnvironmentUtil.TAG_LOOT:
                    HandleLooting(hit.collider.gameObject);
                    break;
            }
        }
    }

    void HandleMovement(Vector3 point)
    {
        if (unitBuffer) unitBuffer.MoveTo(point);
    }

    void HandleSelection(GameObject unit)
    {
        if (unit.TryGetComponent(out UnitController controller)) Select(controller);
        unit.TryGetComponent(out timedBuffer);

    }

    void HandleAttack(GameObject target) 
    {
        if (target.TryGetComponent(out UnitController controller))
        {
            unitBuffer.Attack(controller);
        }
    }

    void HandleLooting(GameObject loot)
    {
        unitBuffer.FetchLoot(loot.transform.position);
    }

    void HandleRewind(TimeSensitive timed)
    {
        if (timed.IsRewinding()) timed.Stop();
        else timed.Rewind();
    }

    public void TakeLoot()
    {
        int loot;
        Loot.GetLoot();
        loot = Loot.LOOT_CLASS;
        lootTotal += loot;
        print("Loot total =" + lootTotal);
    }

    void Select(UnitController unit)
    {
        //deflag the previously selected unit
        if (unitBuffer) unitBuffer.Flag(false);

        unitBuffer = unit;
        unitBuffer.Flag(true);

        timedBuffer = null;
    }

    public static void Log(Object obj, string message)
    {
        print("[" + obj.name + "] - " + message);
    }
}
