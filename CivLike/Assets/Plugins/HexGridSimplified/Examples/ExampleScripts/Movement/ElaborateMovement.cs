using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wunderwunsch.HexGridSimplified
{

    public class ElaborateMovement : Movement
    {
        [SerializeField]
        Mouse mouse = null;
        Vector3Int prevMousePosition = new Vector3Int(-1, -1, -1);
        [SerializeField]
        private GameObject pathMarkerTextPrefab = null;
        [SerializeField]
        private GameObject movementBorderPrefab = null;
        private List<GameObject> pathMarkers;
        private List<GameObject> borderMarkers;
        

        public void Start()
        {
            RemainingMovementPoints = maxMovementPoints;
            pathMarkers = new List<GameObject>();
            borderMarkers = new List<GameObject>();
            map = gameManager.Map;
            Debug.Log(map.Tiles);
            UpdateMovementBorder();
        }

        public void UpdateMovementBorder()
        {
            foreach (var markers in borderMarkers)
            {
                Destroy(markers);

            }
            borderMarkers.Clear();

            Vector3Int entityPosition = HexConverter.WorldPositionToCubeCoord(transform.position);
            var TilesInRange = Pathfinder.GetAllInTurnRange(this, entityPosition);
            HashSet<Vector3Int> edges = Hex.GetBorderEdgesOfSetOfTiles(TilesInRange.Keys);

            foreach (var e in edges)
            {
                int direction = 0;
                if (e.x % 2 == 0) direction = 120;
                else if (e.z % 2 == 0) direction = 240;
                // we don't need to check for  e.y % 2 == 0 as direction is 0 for that and one of the 3 are always 0
                var g = GameObject.Instantiate(movementBorderPrefab, HexConverter.CubeCoordToWorldPosition(e) / 2f, Quaternion.Euler(0, direction, 0));
                g.transform.position = new Vector3(g.transform.position.x, 2, g.transform.position.z);
                borderMarkers.Add(g);

                //the following rotates the edges in such a way that they are always facing inwards
                //fails at the world seam, should be easy enough to fix
                List<Vector3Int> adjacent = Hex.GetAdjacentTilesOfEdge(e,false);
                if(TilesInRange.ContainsKey(adjacent[1]))
                {
                    g.transform.Rotate(0, 180, 0);
                }
            }
        }



        public void Update()
        {
            Vector3Int entityPosition = HexConverter.WorldPositionToCubeCoord(transform.position);
            Vector3Int curMousePosition = mouse.CubePositionSanitized;
            if(curMousePosition != prevMousePosition)
            {
                foreach(var markers in pathMarkers)
                {
                    Destroy(markers);

                }
                pathMarkers.Clear();

                var path = Pathfinder.GetPath(this, entityPosition, curMousePosition, movementRulesData.minimalCost);

                if(path!= null)
                { 
                    foreach(var node in path)
                    {
                        Vector3Int pos = node.position;
                        int turnCost = node.costToThisNode.turnCost;
                        int movementpointCost = node.costToThisNode.movementPointCost;
                        var textMarker = GameObject.Instantiate(pathMarkerTextPrefab, HexConverter.CubeCoordToWorldPosition(pos), Quaternion.identity);
                        textMarker.transform.Translate(Vector3.up);
                        textMarker.transform.localRotation = Quaternion.Euler(90, 0, 0);
                        if (turnCost > 0)
                        {
                            textMarker.GetComponent<TextMesh>().text = "turn: " + turnCost + "\r\nMP:" + movementpointCost;
                        }
                        else
                        {
                            textMarker.GetComponent<TextMesh>().text = "turn: " + turnCost + "\r\nMP:" + (movementpointCost-maxMovementPoints+RemainingMovementPoints);
                        }
                        

                        pathMarkers.Add(textMarker);
                    }
                }
            }

            if(Input.GetMouseButtonDown(1))
            {
                TryMove(entityPosition, curMousePosition);
            }
            prevMousePosition = curMousePosition;
        }

        private void TryMove(Vector3Int entityPosition, Vector3Int target)
        {
            var TilesInRange = Pathfinder.GetAllInTurnRange(this, entityPosition);
            if (TilesInRange.ContainsKey(target))
            {
                int movementCost = TilesInRange[target];
                RemainingMovementPoints -= movementCost;
                transform.position = HexConverter.CubeCoordToWorldPosition(target);
                UpdateMovementBorder();
            }
        }

        public override int CalculateCostBetweenTiles(Vector3Int posA, Vector3Int posB)
        {
            Vector2Int posBOffset = HexConverter.CubeCoordToOffsetCoord(posB);
            Tile tileB = map.Tiles[posBOffset.x, posBOffset.y];
            int terrainID = tileB.BaseTerrain;
            int topoID = tileB.Topography;
            int vegetationID = tileB.Vegetation;
            int terrainCost = movementRulesData.CostByBaseTerrain[terrainID];
            int topoCost = movementRulesData.CostByTopography[topoID];
            int vegetationCost = movementRulesData.CostByVegetation[vegetationID];
            if (terrainCost < 0 || topoCost < 0 || vegetationCost < 0) return -1;
            int movementPointsNeeded = terrainCost + topoCost + vegetationCost;
            return movementPointsNeeded;
        }

        public override bool TileIsWalkable(Vector3Int posA)
        {
            
            Vector2Int posAOffset = HexConverter.CubeCoordToOffsetCoord(posA);
            Tile tileA = map.Tiles[posAOffset.x, posAOffset.y];
            int terrainID = tileA.BaseTerrain;
            int topoID = tileA.Topography;
            int vegetationID = tileA.Vegetation;
            int terrainCost = movementRulesData.CostByBaseTerrain[terrainID];
            int topoCost = movementRulesData.CostByTopography[topoID];
            int vegetationCost = movementRulesData.CostByVegetation[vegetationID];
            if (terrainCost < 0 || topoCost < 0 || vegetationCost < 0) return false;
            return true;
        }

        public override bool TryMove(Vector3Int target)
        {
            bool couldMove = false;
            Dictionary<Vector3Int, int> targets = Pathfinder.GetAllInTurnRange(this, HexConverter.WorldPositionToCubeCoord(this.transform.position));
            if (targets.ContainsKey(target))
            {
                int moveCost = targets[target];
                RemainingMovementPoints -= moveCost;
                this.transform.position = HexConverter.CubeCoordToWorldPosition(target);
                couldMove = true;
                //UPDATE UI
            }
            return couldMove;
        }

        public void ReplenishMovementPoints()
        {
            RemainingMovementPoints = maxMovementPoints;
            UpdateMovementBorder();
        }
    }
}
