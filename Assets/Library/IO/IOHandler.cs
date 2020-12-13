using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOHandler : MonoBehaviour
{

    UnitController unitBuffer;

    bool aUnitSelected;

    [SerializeField] int framerate = 60;
    
    // Start is called before the first frame update
    void Awake()
    {
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

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 150f))
        {
            switch (hit.collider.tag)
            {
                case "movement_tile":
                    Debug.Log("Handling Movement");
                    HandleMovement(hit.collider.gameObject);
                    break;

                case "player_unit":
                    Debug.Log("Handling Selection");
                    HandleSelection(hit.collider.gameObject);
                    break;

                case "enemy_unit":
                    Debug.Log("Handling Attack");
                    HandleAttack(hit.collider.gameObject);
                    break;
            }
        }
    }

    void HandleMovement(GameObject tile)
    {
        unitBuffer.MoveTo(tile.transform.position);
    }

    void HandleSelection(GameObject unit)
    {
        UnitController controller;
        
        if (unit.TryGetComponent(out controller))
        {            
            Select(controller);
        }
        else
        {
            Debug.LogError("No valid controller found on: " + unit.name);
        }       
    }

    void HandleAttack(GameObject target) 
    {
        UnitController targetControllerController;

        if (target.TryGetComponent(out targetControllerController))
        {
            unitBuffer.Attack(targetControllerController);
        }
    }

    void Select(UnitController unit)
    {
        //deflag the previously selected unit
        if (unitBuffer) unitBuffer.Flag(false);

        unitBuffer = unit;
        unitBuffer.Flag(true);
    }
}
