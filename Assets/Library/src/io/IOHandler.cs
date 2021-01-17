using System.Collections;
using System.Collections.Generic;
using Library.src.units;
using Library.src.util;
using UnityEngine;

public class IOHandler : MonoBehaviour
{
    Camera mainCam;

    UnitController unitBuffer;

    bool aUnitSelected;

    int lootTotal;

    [SerializeField] int framerate = 60;
    
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = framerate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.targetFrameRate != framerate) Application.targetFrameRate = framerate;

        if (Input.GetMouseButtonDown(0)) Cast();
    }

    void Cast()
    {       
        RaycastHit hit;

        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, 150f))
        {
            switch (hit.collider.tag)
            {
                case "movement_tile":
                    HandleMovement(hit.collider.gameObject);
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

    void HandleMovement(GameObject tile)
    {
        if (unitBuffer) unitBuffer.MoveTo(tile.transform.position);
    }

    void HandleSelection(GameObject unit)
    {
        if (unit.TryGetComponent(out UnitController controller))
        {            
            Select(controller);
        }
        else
        {
            print("No valid controller found on: " + unit.name);
        }       
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
    }

    public static void Log(Object obj, string message)
    {
        print("[" + obj.name + "] - " + message);
    }
}
