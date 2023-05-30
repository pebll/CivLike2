using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class ElaborateVisionProvider : VisionProvider
    {
        [SerializeField]
        private VisionRulesData baseVisionRules = null;
        [SerializeField]
        private List<bool> ignoreTopographyObstacle = null;
        [SerializeField]
        private List<bool> ignoreVegetationObstacle = null;

        protected override HashSet<Vector2Int> CalculateVisibleTiles()
        {
            HashSet<Vector3Int> markedAsVisible = new HashSet<Vector3Int>();
            Vector3Int pos = HexConverter.WorldPositionToCubeCoord(transform.position);
            Vector2Int posOffset = HexConverter.CubeCoordToOffsetCoord(pos);
            markedAsVisible.Add(pos); //Cell we stand on is always visible

            int modifiedVisionRange = VisionRange + 1;
            // we add one to the base vision range as you can see further than normal in certain conditions (like in civ6)
            int ringSize = modifiedVisionRange;
            int trimEndOfVisionLine = 0;
            if (ringSize % 2 != 0)
            {
                ringSize += 1;
                // this way the ring has always an even numbered Distance to origin. 
                // We need a to check few more lines but this prevents some inconsistencies,
                // where a tile is visible with vision range 4 , invisible with vision range 5, visible with range 6.
                // it might not prevent 100% of those cases but didn't see one yet , while those others were frequent.
                trimEndOfVisionLine = 1;
                //we use the bigger ring to have more and more consistent lines but we don't need to follow them to the end
            }

            int visionStrength = baseVisionRules.VisibilityByTopographyID[map.Tiles[posOffset.x, posOffset.y].Topography];
            //we check how good we can see from what we stand on

            IEnumerable<Vector3Int> targets = Hex.GetRing(pos, ringSize, 1, false);
            foreach (var target in targets)
            {
                List<Vector3Int> linePointsL = Hex.GetLine(pos, target, -0.00001f, 1, trimEndOfVisionLine, true);
                List<Vector3Int> linePointsR = Hex.GetLine(pos, target, +0.00001f, 1, trimEndOfVisionLine, true);
                //first param is LeftTrim => we don't need startCell so it's 1

                List<Vector3Int> visibleL = CheckVisibility(visionStrength, linePointsL);
                List<Vector3Int> visibleR = CheckVisibility(visionStrength, linePointsR);
                //CheckVisibilityAlongLine has the actual Actual Civilization-Like line-of-sight-logic                
                markedAsVisible.UnionWith(visibleL);
                markedAsVisible.UnionWith(visibleR);
            }

            //convert to OFfsetCoordinates
            HashSet<Vector2Int> visibleInOffsetCoords = new HashSet<Vector2Int>();
            foreach (var v in markedAsVisible)
            {
                visibleInOffsetCoords.Add(HexConverter.CubeCoordToOffsetCoord(v));
            }
            return visibleInOffsetCoords;
        }

        private List<Vector3Int> CheckVisibility(int visionStrength, List<Vector3Int> linePoints)
        {
            List<Vector3Int> visible = new List<Vector3Int>();
            int highestObstacleValueSoFar = 0;
            int idxLastSegment = linePoints.Count - 1; //we need that to apply the special logic for the hex out of "normal" vision range
            for (int i = 0; i <= idxLastSegment; i++)
            {
                Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(linePoints[i]);
                Tile t = map.Tiles[offsetCoord.x, offsetCoord.y];
                Vector3Int cellPos = linePoints[i];

                int visibilityTopography = baseVisionRules.VisibilityByTopographyID[t.Topography];
                int visibilityVegetation = baseVisionRules.VisibilityValueByVegetationID[t.Vegetation];
                int totalVisibility = visibilityTopography + visibilityVegetation;

                if (totalVisibility > highestObstacleValueSoFar || visionStrength >= highestObstacleValueSoFar)
                {
                    if (i == idxLastSegment && visibilityTopography <= highestObstacleValueSoFar) break;

                    visible.Add(cellPos);
                }

                int topographyObstructionValue = ignoreTopographyObstacle[t.Topography] ? 0 : visibilityTopography;
                int vegetationObstructionValue = ignoreVegetationObstacle[t.Vegetation] ? 0 : visibilityVegetation;
                int totalObstructionValue = topographyObstructionValue + vegetationObstructionValue;
                highestObstacleValueSoFar = Mathf.Max(highestObstacleValueSoFar, totalObstructionValue);
            }
            return visible;
        }
    }
}
