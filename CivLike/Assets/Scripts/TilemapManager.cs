using Mono.Cecil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapManager : MonoBehaviour
{
    public static TilemapManager Instance;
    private Tilemap _tilemap;
    private GameTile[,] _tiles;
    private int _mapWidth;
    private int _mapHeight;
    private Dictionary<string, TileSO> _tileDict = new Dictionary<string, TileSO>();
    public Dictionary<string, TileSO> TileDict => _tileDict;

    private void Awake()
    {
        Instance= this;
        _tilemap= GetComponent<Tilemap>();
        TileSO[] tileArray = Resources.LoadAll<TileSO>("Tiles");
        foreach(TileSO tileSO in tileArray)
        {
            _tileDict.Add(tileSO.name, tileSO);
        }
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

    public void Load(GameTile[,] tiles = null)
    {     
        if (tiles == null) { tiles = _tiles;}
        foreach (GameTile tile in tiles)
        {
            if (tile != null)
            {
                Vector3Int tilePosition = new Vector3Int(tile.Position.y, tile.Position.x, 0);
                _tilemap.SetTile(tilePosition, tile.Tile);
            }
        }       
    }
}
