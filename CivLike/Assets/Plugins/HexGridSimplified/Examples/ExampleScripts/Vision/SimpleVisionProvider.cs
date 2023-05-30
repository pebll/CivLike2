using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class SimpleVisionProvider : VisionProvider
    {
        [SerializeField]
        private int DebugidxBlockingTerrain = 0;

        protected override HashSet<Vector2Int> CalculateVisibleTiles()
        {
            HashSet<Vector3Int> markedAsVisible = new HashSet<Vector3Int>();

            Vector3Int pos = HexConverter.WorldPositionToCubeCoord(transform.position);
            IEnumerable<Vector3Int> targets = Hex.GetRing(pos, VisionRange,1, false); //can always be map 0 as this ring is just a helper result to draw teh lines

            //TODO: check if we changed aeverything properly
            foreach (var target in targets)
            {
                List<Vector3Int> linePointsL = Hex.GetLine(pos, target, -0.00001f, 1, 0, true);
                List<Vector3Int> linePointsR = Hex.GetLine(pos, target, +0.00001f, 1, 0, true);

                List<Vector3Int> visibleL = CheckVisibility(linePointsL);
                List<Vector3Int> visibleR = CheckVisibility(linePointsR);
                markedAsVisible.UnionWith(visibleR);
                markedAsVisible.UnionWith(visibleL);
                markedAsVisible.Add(pos); //tile the unit itself stands on
            }

            HashSet<Vector2Int> visibleTiles = new HashSet<Vector2Int>();
            foreach (var v in markedAsVisible)
            {
                visibleTiles.Add(HexConverter.CubeCoordToOffsetCoord(v));
            }
            return visibleTiles;
        }

        private List<Vector3Int> CheckVisibility(List<Vector3Int> linePoints)
        {
            List<Vector3Int> visible = new List<Vector3Int>();            
            for (int i = 0; i < linePoints.Count; i++)
            {
                //TODO: we don't need NULLCHECK Here as we sanitized the input already

                Vector3Int cellPos = linePoints[i];
                Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(cellPos);
                int baseTerrainIdx = map.Tiles[offsetCoord.x, offsetCoord.y].BaseTerrain;
                //are there cases where we are out of bounds with the line and return? don't think so, and most likely not both lines at least!
                visible.Add(cellPos);
                if (baseTerrainIdx == DebugidxBlockingTerrain) break;
            }
            return visible;
        }
    }
}
