
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexGridSimplified
{
    public class Map
    {
        public bool WrapsHorizontal { get; private set; }
        public Vector2Int MapSize { get; private set; }
        public MapVisualisation MapVisualisation { get; private set; }
        public Tile[,] Tiles { get; private set; }
        public Dictionary<Vector3Int, Edge> Edges { get; private set; } //Key is sum of CubeCoordinate of Both Adjacent Tiles, that provides an easy way to access Edges
        
        //IDEA : - 1 = undiscovered ; 0 = Discovered/NoVision ; 1 = 1 Unit sees it , 2 = 2 Units see it, this way we can more easily update when a unit gains or loses vision

        public Map( Vector2Int mapSize, Tile[,] tiles, Dictionary<Vector3Int, Edge> edges, bool wrapsHorizontal, MapVisualisation visualisation)
        {
            MapSize = mapSize;
            Tiles = tiles;
            Edges = edges;
            WrapsHorizontal = wrapsHorizontal;
            MapVisualisation = visualisation;
        }

        public void InitVisualisation()
        {
            MapVisualisation.CreateMapGameGeometry(MapSize);
            MapVisualisation.UpdateTileFeatures(Tiles);
            MapVisualisation.UpdateEdges(Edges);           
        }

        public Tile GetTileData(Vector3Int coord)
        {
            Vector2Int offsetCoord = HexConverter.CubeCoordToOffsetCoord(coord);
            return Tiles[offsetCoord.x, offsetCoord.y];
        }

        public Tile GetTileData(Vector2Int coord)
        {
            return Tiles[coord.x, coord.y];
        }

        public void SetTileData(Vector2Int coord, int layer, int value)
        {
            Tile t = Tiles[coord.x, coord.y];
            Tile newTile;
            int baseTerrain = t.BaseTerrain;
            int vegetation = t.Vegetation;
            int topography = t.Topography;
            if(layer == 0)
            {
                newTile = new Tile(value, topography, vegetation);
            }
            else if(layer == 1)
            {
                newTile = new Tile(baseTerrain, value, vegetation);
            }
            else
            {
                newTile = new Tile(baseTerrain, topography, value);
            }
            Tiles[coord.x, coord.y] = newTile;
            MapVisualisation.UpdateTileFeatures(Tiles);
        }

        public void SetEdgeData(Vector3Int edgeCoord)
        {
            if(Edges.ContainsKey(edgeCoord))
            {
                Edges.Remove(edgeCoord);            
            }
            else
            {
                Edge edge = new Edge(1);
                Edges.Add(edgeCoord, edge);
            }
            MapVisualisation.UpdateEdges(Edges);
        }
    }
}
