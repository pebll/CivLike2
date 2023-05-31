using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private TileSO _grasslands;
    [SerializeField] private int _mapWidth = 10;
    [SerializeField] private int _mapHeight = 6;


    void Start()
    {
        GenerateMap(_mapWidth, _mapHeight);
    }

    void GenerateMap(int width, int height)
    {
        TilemapManager.Instance.Setup(_mapWidth, _mapHeight);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Instantiate a new tile
                Vector3Int position = new Vector3Int(x, y, 0);
                GameTile tile = new GameTile(_grasslands, position);        
                TilemapManager.Instance.AddTile(tile);
            }
        }
        TilemapManager.Instance.Load();
    }
    
}
