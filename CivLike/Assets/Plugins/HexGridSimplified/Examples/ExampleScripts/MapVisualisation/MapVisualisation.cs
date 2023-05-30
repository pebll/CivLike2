using UnityEngine;
using System.Collections.Generic;

namespace Wunderwunsch.HexGridSimplified
{
    public abstract class MapVisualisation : MonoBehaviour
    {
        public GameObject Map { get; protected set; }
        public abstract void CreateMapGameGeometry(Vector2Int mapSize);
        public abstract void UpdateTileFeatures(Tile[,] cells);
        public abstract void UpdateEdges(Dictionary<Vector3Int, Edge> edgesByCoord);
        public abstract void UpdateFogOfWar(int[,] visibility);
    }
}