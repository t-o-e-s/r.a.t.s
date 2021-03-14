using Library.src.time;
using Library.src.units.control;
using Library.src.util;
using UnityEngine;

namespace Library.src.io
{
    public class IOHandler : MonoBehaviour
    {
        Camera mainCam;

        //buffers
        UnitController unitBuffer;
        TimeSensitive timedBuffer;

        Time time;

        bool aUnitSelected;
        int lootTotal;

        [SerializeField] int framerate = 60;
    
        void Awake()
        {
            mainCam = Camera.main;
        
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = framerate;
        }

        void Update()
        {
            if (Application.targetFrameRate != framerate) Application.targetFrameRate = framerate;
        
            if (Input.GetMouseButtonDown(0)) Cast();
        
            //reverse time
            if (Input.GetKeyDown(KeyCode.R)) HandleRewind(timedBuffer);
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
            if (unitBuffer) unitBuffer.Move(tile.transform.position);
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

        void Select(UnitController unit)
        {
            //deflag the previously selected unit
            if (unitBuffer) unitBuffer.Flag(false);

            unitBuffer = unit;
            unitBuffer.Flag(true);

            timedBuffer = null;
        }
    }
}
