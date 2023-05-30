using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{


    public class ClosestEdgeAndAdjacentTiles : MonoBehaviour
    {
        [SerializeField]
        GameObject tileMarkerA = null;
        [SerializeField]
        GameObject tileMarkerB = null;
        [SerializeField]
        GameObject edgeMarker = null;
        [SerializeField]
        Mouse mouse = null;


        // Update is called once per frame
        void Update()
        {
            Vector3Int closestEdge = HexConverter.WorldPositionToClosestEdge(mouse.WorldPositionWrapped);
            List<Vector3Int> adjacentTiles = Hex.GetAdjacentTilesOfEdge(closestEdge,true);

            float edgeDirection = Hex.GetEdgeDirectionAngle(closestEdge);
            Vector3 edgeWorldPos = HexConverter.EdgeCoordToWorldPosition(closestEdge);

            edgeMarker.transform.position = edgeWorldPos;
            edgeMarker.transform.rotation = Quaternion.Euler(90, edgeDirection, 0);

            if(adjacentTiles.Count > 0)
            { 
                tileMarkerA.transform.position = HexConverter.CubeCoordToWorldPosition(adjacentTiles[0]);
            }
            else tileMarkerA.transform.position = new Vector3(-100, -100, -100);

            if (adjacentTiles.Count == 2)
            {
                tileMarkerB.transform.position = HexConverter.CubeCoordToWorldPosition(adjacentTiles[1]);
            }
            else tileMarkerB.transform.position = new Vector3(-100, -100, -100);
        }
    }
}
