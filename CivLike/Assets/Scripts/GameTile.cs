using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameTile
{
    private Tile _tile;
    private string _description;
    private int _yield;
    private string _name;
    private Vector3Int _position;
    public Vector3Int Position => _position;
    public string Description => _description;
    
    public GameTile(TileSO tileSO, Vector3Int position)
    {
        // Tile properties
        _tile = tileSO.Tile;
        _description = tileSO.Description;
        _yield = tileSO.Yield;
        _name = tileSO.name;
  

        // Tile position
        _position = position;
        Vector3Int tilePosition = new Vector3Int(position.y, position.x, 0);
        TilemapManager.Instance.SetTile(tilePosition, _tile);
    }


    
}