using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class SimpleConeVisionProvider : VisionProvider
    {
        private Vector3Int lookDirection = new Vector3Int(1, 0, -1);
        [SerializeField]
        private int halfVisionConeAngle = 0;
        [SerializeField]
        private int DebugidxBlockingTerrain = 0;

        protected override HashSet<Vector2Int> CalculateVisibleTiles()
        {
            HashSet<Vector3Int> markedAsVisible = new HashSet<Vector3Int>();

            Vector3Int pos = HexConverter.WorldPositionToCubeCoord(transform.position);
            
            IEnumerable<Vector3Int> targets = Hex.GetRing(pos, VisionRange,1, false); //can always be map 0 as this ring is just a helper result to draw teh lines

            foreach (var target in targets)
            {
                Vector3 targetWorldPos = HexConverter.CubeCoordToWorldPosition(target);
                Vector3 lookWorldPos = HexConverter.CubeCoordToWorldPosition((pos + lookDirection)); //could also use that as axis too;
                float angle = Vector3.Angle(targetWorldPos-transform.position,lookWorldPos-transform.position);
                

                //Debug.Log(angle);
                if(Mathf.Abs(angle) > halfVisionConeAngle) continue;

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

                Vector3Int cellPos = linePoints[i];
                Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(cellPos);
                int baseTerrainIdx = map.Tiles[offsetCoord.x, offsetCoord.y].BaseTerrain;
                visible.Add(cellPos);
                if (baseTerrainIdx == DebugidxBlockingTerrain) break;
            }
            return visible;
        }

        public void RotateClockwise()
        {
            lookDirection = Hex.Rotate60DegreeClockwise(Vector3Int.zero, lookDirection);
            UpdateVisibleTiles();
        }

        public void RotateCounterClockwise()
        {
            lookDirection = Hex.Rotate60DegreeCounterClockwise(Vector3Int.zero, lookDirection);
            UpdateVisibleTiles();
        }
           
    }
}
