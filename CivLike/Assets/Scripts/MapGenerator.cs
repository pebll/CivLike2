using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{    
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 6;
    private const int NOISEMAP_AMOUNT = 4;
    private float[,,] _noiseMaps;
    private TileSO[,] _chosenTiles;
    private Dictionary<string, TileSO> _tileDict;


    void Start()
    {
        _tileDict = TilemapManager.Instance.TileDict;
        GenerateMap(_mapWidth, _mapHeight);      
    }

    void GenerateMap(int width, int height)
    {
        // Setup
        _noiseMaps = GenerateNoiseMaps(); // 0: Water level 1: Temperature, 2: Elevation 3: Vegetation,        
        _chosenTiles = new TileSO[width, height];       
        TilemapManager.Instance.Setup(_mapWidth, _mapHeight);
        // Choose Tiles
        for(int i = 0; i < NOISEMAP_AMOUNT; i++)
        {
            switch(i)
            {
                case 0: GenerateWaterLevel(i);
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
                GameTile tile = new GameTile(chosenTile, position);        
                TilemapManager.Instance.AddTile(tile);
            }
        }
        // Load Tiles
        TilemapManager.Instance.Load();
    }

    private void GenerateWaterLevel(int mapIndex)
    {
        // 1 = Earth 0 = Ocean
        for (int x = 0; x < _mapWidth; x++)
        {
            for (int y = 0; y < _mapHeight; y++)
            {
                float noiseValue = _noiseMaps[mapIndex, x, y];
                if (noiseValue < 0.1f)
                {
                    _chosenTiles[x, y] = _tileDict["Ocean"];
                }
                else if (noiseValue < 0.4f)
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
                if (noiseValue < 0.3f)
                {
                    //_chosenTiles[x, y] = _grasslands;
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
                if (noiseValue < 0.3f)
                {
                    //_chosenTiles[x, y] = _grasslands;
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
                if (noiseValue < 0.3f)
                {
                    //_chosenTiles[x, y] = _grasslands;
                }
            }
        }
    }
    private float[,,] GenerateNoiseMaps(int amount = NOISEMAP_AMOUNT, int scale = 5)
    {
        float[,,] noiseMaps = new float[amount, _mapWidth, _mapHeight];
        for(int i = 0; i < amount; i++)
        {
            // generate a perlin noise map with random offsets that is then represented on the array
            float offsetX = Random.Range(0, 1000000f);
            float offsetY = Random.Range(0, 1000000f);
            for (int x = 0; x < _mapWidth; x++)
            {
                for (int y = 0; y < _mapHeight; y++)
                {
                    noiseMaps[i, x, y] = Mathf.PerlinNoise(offsetX + (float)x / _mapWidth * scale, offsetY + (float)y / _mapHeight * scale);
                }
            }
        }
        return noiseMaps;       
    }
    
}
