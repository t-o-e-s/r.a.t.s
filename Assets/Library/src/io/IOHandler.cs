using Library.src.units;
using UnityEngine;

namespace Library.src.io
{
    public class IOHandler : MonoBehaviour
    {
        Camera cam;
        UnitController unitBuffer;

        [SerializeField] int framerate = 60;
        
        bool aUnitSelected;


        void Awake()
        {
            cam = Camera.main;
            
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = framerate;
        }

        void Update()
        {
            if (Application.targetFrameRate != framerate) Application.targetFrameRate = framerate;

            if (Input.GetMouseButtonDown(0)) Cast();
        }

        void Cast()
        {       
            RaycastHit hit;

            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, 150f))
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
            unitBuffer?.MoveTo(tile.transform.position);
        }

        void HandleSelection(GameObject unit)
        {
            if (unit.TryGetComponent(out UnitController controller)) Select(controller);
        }

        void HandleAttack(GameObject target) 
        {
            if (target.TryGetComponent(out UnitController targetController)) unitBuffer.Attack(targetController);
        }

        void Select(UnitController unit)
        {
            if (unitBuffer) unitBuffer.Flag(false);

            unitBuffer = unit;
            unitBuffer.Flag(true);
        }
    }
}
