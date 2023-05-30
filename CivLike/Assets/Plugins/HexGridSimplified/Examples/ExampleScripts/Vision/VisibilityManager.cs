using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wunderwunsch.HexGridSimplified
{
    public class VisibilityManager : MonoBehaviour
    {
        [SerializeField]
        private GameManager gameManager = null;
        private Map map;
        [SerializeField]
        private MapVisualisation mapVisualisation = null;
        private int[,] visibilityOfTiles;

        
        public void UpdateVisibility(IEnumerable<Vector2Int> TilesVisionLost, IEnumerable<Vector2Int> TilesVisionGained)
        {
            if (map == null) map = gameManager.Map;
            if(visibilityOfTiles == null)
            {
                visibilityOfTiles = new int[map.MapSize.x, map.MapSize.y];
                for (int y = 0; y < visibilityOfTiles.GetLength(1); y++)
                    for (int x = 0; x < visibilityOfTiles.GetLength(0); x++)
                    {
                        visibilityOfTiles[x, y] = -1;
                    }
            }

            foreach (Vector2Int lost in TilesVisionLost)
            {
                visibilityOfTiles[lost.x, lost.y]--;
            }

            foreach (Vector2Int gained in TilesVisionGained)
            {
                visibilityOfTiles[gained.x, gained.y] = Mathf.Max(1, visibilityOfTiles[gained.x, gained.y] + 1);
            }
            mapVisualisation.UpdateFogOfWar(visibilityOfTiles);
        }
    }    
}
