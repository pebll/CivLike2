using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Wunderwunsch.HexGridSimplified
{
    public class SimpleRandomMapGenerator : MapGenerator
    {
        [SerializeField]
        private int baseTerrainMaxValue = 4;
        [SerializeField]
        private int MinRollForHill = 100;
        [SerializeField]
        private int MinRollForMountain = 100;
        [SerializeField]
        private int MinRollForForest = 100;


        public override Map GenerateMap()
        {
            Tile[,] tiles = GenerateTiles();
            Dictionary<Vector3Int, Edge> edges = GenerateEdges();

            Map map = new Map(mapSize,  tiles, edges, wrapHorizontal, mapVisualisation);
            Hex.SetMapAttributes(mapSize, wrapHorizontal);            
            return map;
        }

        private Tile[,] GenerateTiles()
        {
            Tile[,] tiles = new Tile[mapSize.x, mapSize.y];

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    int terrainValue = Random.Range(0, baseTerrainMaxValue);

                    int topographyRandomValue = Random.Range(0, 100);
                    int vegetationRandomValue = Random.Range(0, 100);
                    int topographyValue = 0;
                    int vegetationValue = 0;

                    if (topographyRandomValue >= MinRollForHill) topographyValue = 1;
                    if (topographyRandomValue >= MinRollForMountain) topographyValue = 2;
                    if (vegetationRandomValue >= MinRollForForest) vegetationValue = 1;

                    if (terrainValue < 1) //water tiles for now;
                    {
                        vegetationValue = 0;
                        topographyValue = 0;
                    }
                    if (topographyValue == 2) vegetationValue = 0; //2 = Mountain for now, we don't put anything on mountains!
                    Tile t = new Tile(terrainValue, topographyValue, vegetationValue);
                    tiles[x, y] = t;
                }

            return tiles;
        }
        private Dictionary<Vector3Int, Edge> GenerateEdges()
        {
            Dictionary<Vector3Int, Edge> edgesByCoord = new Dictionary<Vector3Int, Edge>();
            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    int rng = Random.Range(1, 11);
                    if (rng > 8)
                    {
                        Vector3Int c = HexConverter.OffsetCoordToCubeCoord(new Vector2Int(x, y));
                        List<Vector3Int> neighbours = Hex.GetNeighbours(c, false);
                        int randomIdx = Random.Range(0, 6);
                        Vector3Int other = neighbours.ElementAt(randomIdx);
                        Vector3Int edgeCoord = Hex.GetEdgeCoordBetween(c, other);

                        if (edgesByCoord.ContainsKey(edgeCoord)) continue; //the other Tile already created an edge there which we don't overwrite

                        int edgeType = 1;                     
                        Edge edge = new Edge(edgeType);
                        edgesByCoord.Add(edgeCoord, edge);
                    }
                }
            return edgesByCoord;
        }
    }
}
