using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Wunderwunsch.HexGridSimplified
{
    public class MapEditor : MonoBehaviour
    {
        [SerializeField]
        Dropdown mode = null;
        [SerializeField]
        Dropdown TerrainType = null;
        [SerializeField]
        Mouse mouse = null;
        [SerializeField]
        GameManager gameManager = null;
        Map map;
        [SerializeField]
        EventSystem eventSystem = null;
        [SerializeField]
        bool mirrorVertical = false;
        [SerializeField]
        bool mirrorHorizontal = false;

        public void Update()
        {
            if(Input.GetMouseButtonDown(0) && !(eventSystem.IsPointerOverGameObject()))
            {
                ChangeMapElement();
            }
        }

        public void ChangeMapElement()
        {
            if (map == null) map = gameManager.Map;
            Debug.Log(map);
            if(mode.value == 0)
            {
                ChangeTile(0);
            }
            else if(mode.value == 1)
            {
                ChangeTile(1);
            }

            else if (mode.value == 2)
            {
                ChangeTile(2);
            }

            else if(mode.value == 3)
            {
                ChangeEdge();
            }

        }

        public void ChangeTile(int layer)
        {
            Vector2Int tilePos = mouse.OffsetPositionSanitized;
            int value = TerrainType.value;
            if(layer == 1)
            {
                value = Mathf.Min(2, value);
            }
            else if (layer == 2)
            {
                value = Mathf.Min(1, value);
            }

            var tiles = Hex.GetMirroredTileCoordinates(tilePos, mirrorHorizontal, mirrorVertical, true, true);

            foreach( var t in tiles)
            { 
                map.SetTileData(t, layer, value);
            }
        }

        public void ChangeEdge()
        {            
            Vector3Int edge = HexConverter.WorldPositionToClosestEdge(mouse.WorldPositionWrapped);           
            map.SetEdgeData(edge);
        }
    }
}
