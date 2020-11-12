using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOHandler : MonoBehaviour
{

    HashSet<UnitController> unitBuffer = new HashSet<UnitController>();
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
                    HandleMovement(hit.collider.gameObject);
                    break;

                case "player_unit":
                    HandleSelection(hit.collider.gameObject);
                    break;
            }
        }
    }

    void HandleMovement(GameObject tile)
    {
        foreach(UnitController unit in unitBuffer)
        {
            unit.Move(tile.transform.position);
        }
    }

    void HandleSelection(GameObject unit)
    {
        UnitController controller;

        if (unit.TryGetComponent(out controller))
        {
            if (unitBuffer.Contains(controller))
            {
                if (aUnitSelected == true)
                {
                    //removing unit from the buffer & turning flag off
                    if (unitBuffer.Remove(controller))
                    {
                        controller.Flag(false);
                        aUnitSelected = false;
                    }
                }
            }
            else
            {
                if (aUnitSelected == false)
                {
                    //adding unit to buffer & turning flag on
                    if (unitBuffer.Add(controller))
                    {
                        controller.Flag(true);
                        aUnitSelected = true;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("[ERROR] - No valid controller found on: " + unit.name);
        }       
    }
}
