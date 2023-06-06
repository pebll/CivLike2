using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private bool _sandbox = false;
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 6;
    private const int NOISEMAP_AMOUNT = 4;
    private float[,,] _noiseMaps;
    private TileSO[,] _chosenTiles;
    private Dictionary<string, TileSO> _tileDict;
    [Space(10)]
    [SerializeField][Range(0, 1)] private float _oceanAmount = 0.1f;
    [SerializeField][Range(0, 1)] private float _seaAmount =0.4f;
    [SerializeField][Range(0, 1)] private float _desertAmount = 0.3f;
    [SerializeField][Range(0, 1)] private float _grasslandsAmount = 0.7f;
    [SerializeField][Range(0, 1)] private float _mountainAmount = 0.2f;
    [SerializeField][Range(0, 1)] private float _hillsAmount = 0.4f;
    [SerializeField][Range(0, 1)] private float _forestAmount = 0.2f;
    [SerializeField][Range(0, 1)] private float _groveAmount = 0.4f;


    [Space(10)]
    [SerializeField][Range(0, 5)] private float _WaterLevelScale = 2.5f;
    [SerializeField][Range(0, 5)] private float _TemperatureScale = 2.5f;
    [SerializeField][Range(0, 5)] private float _ElevationScale = 2.5f;
    [SerializeField][Range(0, 5)] private float _VegetationScale = 2.5f;

    [Space(10)]
    [SerializeField][Range(0, 1000000)] private int seed;

    private float[] _scales;


    void Start()
    {
        _scales = new float[] { _WaterLevelScale, _TemperatureScale, _ElevationScale, _VegetationScale };
        _tileDict = TilemapManager.Instance.TileDict;       
        GenerateMap(_mapWidth, _mapHeight, seed);      
    }

    private void Update()
    {
        if (_sandbox)
        {
            _scales = new float[] { _WaterLevelScale, _TemperatureScale, _ElevationScale, _VegetationScale };
            GenerateMap(_mapWidth, _mapHeight, seed);
        }
        
    }



    void GenerateMap(int width, int height, int seed = 0)
    {
        // Setup
        if (seed == 0) { seed = Random.Range(0, 1000000); }
        Random.InitState(seed);
        _noiseMaps = GenerateNoiseMaps(_scales); // 0: Water level 1: Temperature, 2: Elevation 3: Vegetation,        
        _chosenTiles = new TileSO[width, height];       
        TilemapManager.Instance.Setup(_mapWidth, _mapHeight);
        // Choose Tiles
        for(int i = 0; i < NOISEMAP_AMOUNT; i++)
        {
            switch(i)
            {
                case 0: GenerateWaterLevel(i);
                    break;
                case 1: GenerateTemperature(i);
                    break;
                case 2: GenerateElevation(i);
                    break;
                case 3: GenerateVegetation(i);
                    break;
            }
        }
        // Instantiate Tiles
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileSO chosenTile = _chosenTiles[x,y];
                Vector3Int position = new Vector3Int(x, y, 0);
                GameTile tile = new GameTile(chosenTile, position, !_sandbox);        
                TilemapManager.Instance.AddTile(tile);
            }
        }
        // Load Tiles
        TilemapManager.Instance.Load();
        UIManager.Instance.ToggleTileResourceDisplays();
    }

    private void GenerateWaterLevel(int mapIndex)
    {
        // 1 = Earth 0 = Ocean
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float noiseValue = _noiseMaps[mapIndex, x, y];
                if (noiseValue < _oceanAmount)
                {
                    _chosenTiles[x, y] = _tileDict["Ocean"];
                }
                else if (noiseValue < _seaAmount)
                {
                    _chosenTiles[x, y] = _tileDict["Sea"];
                }
                else
                {
                    _chosenTiles[x, y] = _tileDict["Grasslands"];
                }
            }
        }
    }

    private void GenerateTemperature(int mapIndex)
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float noiseValue = _noiseMaps[mapIndex, x, y];
                switch (_chosenTiles[x, y].name)
                {
                    case "Grasslands":
                        if (noiseValue < _desertAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["Desert"];
                        }
                        else if (noiseValue >= _grasslandsAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["Tundra"];
                        }
                        break;
                    case "Sea":
                        if (noiseValue >= _grasslandsAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["Ice"];
                        }
                    break;
                }                                       
            }
        }
    }

    private void GenerateElevation(int mapIndex)
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float noiseValue = _noiseMaps[mapIndex, x, y];
                switch (_chosenTiles[x, y].name)
                {
                    case "Grasslands":
                        if (noiseValue < _mountainAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["GrasslandsMountain"];
                        }
                        else if (noiseValue < _hillsAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["GrasslandsHills"];
                        }
                        break;
                    case "Desert":
                        if (noiseValue < _mountainAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["DesertMountain"];
                        }
                        else if (noiseValue < _hillsAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["DesertHills"];
                        }
                        break;
                    case "Tundra":
                        if (noiseValue < _mountainAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["TundraMountain"];
                        }
                        else if (noiseValue < _hillsAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["TundraHills"];
                        }
                        break;

                }
            }
        }
    }

    private void GenerateVegetation(int mapIndex)
    {
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float noiseValue = _noiseMaps[mapIndex, x, y];
                switch (_chosenTiles[x, y].name)
                {
                    case "Grasslands":
                        if (noiseValue < _forestAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["GrasslandsForest"];
                        }
                        else if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["GrasslandsGrove"];
                        }
                        break;
                    case "Desert":
                        if (noiseValue < _forestAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["DesertForest"];
                        }
                        else if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["DesertGrove"];
                        }
                        break;
                    case "Tundra":
                        if (noiseValue < _forestAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["TundraForest"];
                        }
                        else if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["TundraGrove"];
                        }
                        break;                   
                    case "GrasslandsHills":
                        if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["GrasslandsHillsGrove"];
                        }
                        break;
                    case "DesertHills":
                        if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["DesertHillsGrove"];
                        }
                        break;
                    case "TundraHills":                            
                        if (noiseValue < _groveAmount)
                        {
                            _chosenTiles[x, y] = _tileDict["TundraHillsGrove"];
                        }
                        break;



                }
            }
        }
    }
   
    private float[,,] GenerateNoiseMaps(float[] scales)
    {
        float[,,] noiseMaps = new float[NOISEMAP_AMOUNT, _mapWidth, _mapHeight];
        for(int i = 0; i < NOISEMAP_AMOUNT; i++)
        {
            float scale = scales[i];
            // generate a perlin noise map with random offsets that is then represented on the array
            float offsetX = Random.Range(0, 1000000f);
            float offsetY = Random.Range(0, 1000000f);
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    noiseMaps[i, x, y] = Mathf.PerlinNoise(offsetX + (float)x  / 10 * scale, offsetY + (float)y / 10 * scale);
                }
            }
        }
        return noiseMaps;       
    }
    
}
