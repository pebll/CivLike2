using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SD = System.Diagnostics;



namespace Wunderwunsch.HexGridSimplified
{
    public class SimpleMapVisualisation : MapVisualisation
    {
        public delegate Vector3 TileWorldPositionCalculator(Vector2Int pos);

        [SerializeField]
        private Material mapMaterial = null; //needs to use the Wunderwunsch/SimpeMapVisualisation Shader
        [SerializeField]
        private int textureAtlasDimensions = 0; // every texture has to have equal amount of rows and columns.
        private int vertsPerTile;
        [SerializeField]
        bool tilesHaveCentroid = false;
        [SerializeField]
        private GameObject riverPrefab = null;
        [SerializeField]
        private bool drawEdges = false;         //muss evtl public sein, wird sich zeigen
        private List<GameObject> edgeGameObjects;

        ///Creates a Gameobject with a Mesh containing all the tiles
        public override void CreateMapGameGeometry(Vector2Int mapSize)
        {
            vertsPerTile = 6;
            TileWorldPositionCalculator offsetCalculator = HexConverter.OffsetCoordToWorldPosition;
            List<Polygon> polygons = new List<Polygon>();
            Polygon hexagon = Polygon.CreateNGon(vertsPerTile, Vector3.forward, Vector3.up, tilesHaveCentroid);
            vertsPerTile = hexagon.Vertices.Length;

            for (int y = 0; y < mapSize.y; y++)
                for (int x = 0; x < mapSize.x; x++)
                {
                    Vector3 tileOffset = offsetCalculator(new Vector2Int(x, y));
                    Polygon p = hexagon.OffsetPosition(tileOffset);
                    polygons.Add(p);
                }

            Polygon combined = Polygon.Combine(polygons);

            Map = new GameObject();
            Map.AddComponent<MeshFilter>();
            Map.AddComponent<MeshRenderer>();

            Map.GetComponent<MeshFilter>().mesh = combined.ToMesh();
            Map.GetComponent<MeshRenderer>().material = mapMaterial;
            Map.name = "Map";
        }

        /// <summary>
        /// Assigns
        /// </summary>
        public override void UpdateTileFeatures(Tile[,] cells)
        {
            if (mapMaterial.shader.name != "Wunderwunsch/SimpleMapVisualisation") //
            {
                Debug.Log("This Map Visualisation only works properly with the SimpleMapVisualisation Shader");
            }

            Mesh m = Map.GetComponent<MeshFilter>().mesh;
            Vector2[] uvs = new Vector2[m.vertexCount]; //we have 1 uv per Vertex per UVset // base layer
            Vector2[] uvs2 = new Vector2[m.vertexCount]; //we have 1 uv per Vertex per UVset //vegetation layer
            Vector2[] uvs3 = new Vector2[m.vertexCount]; //we have 1 uv per Vertex per UVset //topography layer
            //
            //Debug.Log("UVamount: " + uvs.Length);

            Vector2[][] uvsByValue = CalculateTextureAtlasUVs(textureAtlasDimensions, tilesHaveCentroid); //we don't need to use a dict as our values are 0 to N anyways so List is faster
            int increment = 0;

            for (int y = 0; y < cells.GetLength(1); y++)
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    for (int v = 0; v < vertsPerTile; v++)
                    {
                        int bVal = cells[x, y].BaseTerrain;
                        int fVal = cells[x, y].Vegetation;
                        int tVal = cells[x, y].Topography;

                        uvs[v + increment] = uvsByValue[bVal][v];
                        uvs2[v + increment] = uvsByValue[fVal][v]; // for now we assume same Dimension for FeatureLayerTerrain
                        uvs3[v + increment] = uvsByValue[tVal][v]; // for now we assume same Dimension for FeatureLayerTerrain
                    }
                    increment += vertsPerTile;
                }
            m.uv = uvs;
            m.uv2 = uvs2;
            m.uv3 = uvs3;
        }

        public override void UpdateFogOfWar(int[,] visibility)
        {
            Mesh m = Map.GetComponent<MeshFilter>().mesh;
            Color32[] vertColor = new Color32[m.vertexCount];
            int increment = 0;

            for (int y = 0; y < visibility.GetLength(1); y++)
                for (int x = 0; x < visibility.GetLength(0); x++)
                {
                    int visibilityNumber = visibility[x, y];
                    Color value;
                    if (visibilityNumber == -1) value = new Color32(0, 0, 0, 0);
                    else if (visibilityNumber == 0) value = new Color32(110, 110, 110, 255); //we use the
                    else value = new Color32(255, 255, 255, 255);
                    for (int v = 0; v < vertsPerTile; v++)
                    {
                        vertColor[v + increment] = value;
                    }
                    increment += vertsPerTile;
                }
            m.colors32 = vertColor;
        }

        public override void UpdateEdges(Dictionary<Vector3Int, Edge> edgesByCoord) 
        {
            if (edgeGameObjects == null) edgeGameObjects = new List<GameObject>();

            foreach(var edge in edgeGameObjects)
            {
                Destroy(edge);
            }
            edgeGameObjects.Clear();

            if (!drawEdges) return;
            foreach (var kvp in edgesByCoord)
            {
                Vector3 worldPos = HexConverter.CubeCoordToWorldPosition(kvp.Key);
                worldPos = new Vector3(worldPos.x, 1, worldPos.z);
                float edgeDirection = Hex.GetEdgeDirectionAngle(kvp.Key);
                int edgeType = kvp.Value.EdgeType;
                GameObject edgePrefab = riverPrefab;                
                var instance = GameObject.Instantiate(edgePrefab, worldPos / 2f, Quaternion.Euler(90, edgeDirection, 0)); //EdgeCoords are Sum of both Adjacent Tiles, that's why we divide By2
                edgeGameObjects.Add(instance);
            }
        }

        /// <summary>
        /// Calculates the UV coordinates for each possible cell value
        /// Every Texture in the material should have the same size and same tiling!
        /// Use Square Textures, everything else is bad anyways
        /// </summary>
        /// <returns></returns>
        private Vector2[][] CalculateTextureAtlasUVs(int dimensions, bool hasCentroid)
        {
            Vector2[][] uvCoordsByID = new Vector2[dimensions * dimensions][];

            Texture texture = mapMaterial.GetTexture("_MainTex");
            Vector2Int baseTexSize = new Vector2Int(texture.width, texture.height);

            int sizePerTexSquare = texture.width / dimensions;
            Vector2 midpointOfSquare;

            int radius = sizePerTexSquare / 2;
            for (int y = 0; y < dimensions; y++)
                for (int x = 0; x < dimensions; x++)
                {
                    Vector2[] uvs = tilesHaveCentroid ? new Vector2[7] : new Vector2[6];
                    int t = x + dimensions * y;
                    midpointOfSquare = new Vector2(sizePerTexSquare / 2 + sizePerTexSquare * x, sizePerTexSquare / 2 + sizePerTexSquare * y);

                    float anglePerStep = 360f / 6;

                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 v = (Quaternion.AngleAxis(anglePerStep * i, Vector3.forward) * Vector2.up) * radius * 0.98f;
                        uvs[i] = (midpointOfSquare + v) / texture.width;
                    }
                    if (tilesHaveCentroid) uvs[6] = midpointOfSquare / texture.width;
                    uvCoordsByID[x + (y * dimensions)] = uvs;
                }
            return uvCoordsByID;
        }
    }
}
