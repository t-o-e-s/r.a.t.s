using System.Security.Cryptography;
using Library.src.units;
using UnityEngine;

namespace Library.src.ui
{
    public class UnitInfoHandler : MonoBehaviour
    {
        GameObject playerPanel;
        GameObject enemyPanel;
        GameObject parent;

        RectTransform playerBaseTransform;
        RectTransform enemyBaseTransform;

        [SerializeField] int uISpacing = 100;
    
        void Awake()
        {
            parent = GameObject.Find("unit_info");
            var player = GameObject.Find("friendly_panel");
            var enemy = GameObject.Find("enemy_panel");
            playerBaseTransform = player.GetComponent<RectTransform>();
            enemyBaseTransform = enemy.GetComponent<RectTransform>();
        
            GenerateUI(playerBaseTransform, GameObject.FindGameObjectsWithTag("player_unit"));
            GenerateUI(enemyBaseTransform, GameObject.FindGameObjectsWithTag("enemy_unit"));
        
            Destroy(this);
        }

        void GenerateUI(RectTransform protoPanel, GameObject[] units)
        {
            var proto = protoPanel.gameObject;
            var xPos = protoPanel.position.x;
            var yPos = protoPanel.position.y;
            int i = 0;
            foreach (var unit in units)
            {
                UnitController uC;
                if (!unit.TryGetComponent(out uC)) continue;
                GameObject newPanel = Instantiate(proto, parent.transform);
                newPanel.GetComponent<RectTransform>().SetParent(parent.transform, true);
                newPanel.GetComponent<RectTransform>().position.Set(yPos, xPos, 0);
                newPanel.name = newPanel.name.Replace("(Clone)", "_" + i++);
                uC.UpdateUI(newPanel);
                yPos += uISpacing;
            }
            Destroy(protoPanel.gameObject);
        }
    }
}
