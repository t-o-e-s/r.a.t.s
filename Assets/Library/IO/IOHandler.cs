using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOHandler : MonoBehaviour
{

    UnitController unitBuffer;

    bool aUnitSelected;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //player left click
        if (Input.GetMouseButtonDown(0))
        {
            Cast();
        }
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
        unitBuffer.Move(tile.transform.position);
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
        UnitController targetController;

        if (target.TryGetComponent(out targetController))
        {
            unitBuffer.Attack(targetController);
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
