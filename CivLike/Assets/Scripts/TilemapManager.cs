using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance;
    private Tilemap _tilemap;
    private GameTile[,] _tiles;
    private int _mapWidth;
    private int _mapHeight;

    private void Awake()
    {
        Instance= this;
        _tilemap= GetComponent<Tilemap>();       
    }

    public void Setup(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth;   
        _mapHeight = mapHeight;
        _tiles = new GameTile[_mapWidth, _mapHeight];
    }
    public void SetTile(Vector3Int position, Tile tile)
    {
        _tilemap.SetTile(position, tile);
    }

    public void AddTile(GameTile tile)
    {      
        _tiles[tile.Position.x, tile.Position.y] = tile;
    }
}
