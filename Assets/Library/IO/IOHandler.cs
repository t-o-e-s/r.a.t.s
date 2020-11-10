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

                case "enemy_unit":
                    HandleAttack(hit.collider.gameObject);
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
            if (!unitBuffer.Contains(controller))
            {
                ClearBuffer();
                Select(controller, true);
            }
            else
            {
                Select(controller, false);
            }
        }
        else
        {
            Debug.LogError("[ERROR] - No valid controller found on: " + unit.name);
        }       
    }

    void HandleAttack(GameObject target) 
    {
        UnitController targetController;

        if (target.TryGetComponent(out targetController))
        {
            if (targetController.InCombat())
            {
                foreach (UnitController u in unitBuffer)
                {
                    targetController.combat.AddRats(u);
                    u.Move(target.transform.position);
                }
            }
            else
            {
                GameObject resolver = new GameObject();
                CombatResolver combat = resolver.AddComponent<CombatResolver>();

                foreach (UnitController u in unitBuffer)
                {
                    combat.AddRats(u);
                    u.Move(target.transform.position);
                }

                combat.AddEnemy(targetController);
            }
        }
    }

    void ClearBuffer()
    {
        foreach (UnitController unit in unitBuffer)
        {
            Select(unit, false);
        }
    }

    void Select(UnitController unit, bool active)
    {
        if (active) unitBuffer.Add(unit);
        else unitBuffer.Remove(unit);

        unit.Flag(active);
    }

    bool IsUnitSelected()
    {
        return unitBuffer.Count > 0;
    }
}
