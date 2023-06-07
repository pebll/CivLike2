using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GameTile
{
    
    private Tile _tile;
    private string _description;
    private Dictionary<ResourceManager.Resource, int> _yield;
    private string _name;
    private Vector3Int _position;
    private WorldTile _worldTile;
    private Building _building;
    public Vector3Int Position => _position;
    public string Description => _description;
    public Tile Tile => _tile;
    public WorldTile WorldTile => _worldTile;



    public Dictionary<ResourceManager.Resource, int> Yield => _yield;
    
    public GameTile(TileSO tileSO, Vector3Int position, bool generateWorldTile = true)
    {
        // Tile properties
        _tile = tileSO.Tile;
        _description = tileSO.Description;
        _yield = ResourceManager.Instance.GetResourceDict(tileSO.Yield);
        _name = tileSO.name;
  

        // Tile position
        _position = position;

        //Tile WorldObject
        if (generateWorldTile)
        {
            _worldTile = Object.Instantiate(TilemapManager.Instance.WorldTilePrefab, GameObject.FindGameObjectWithTag("Tiles").transform).GetComponent<WorldTile>();
            _worldTile.Setup(this);
        }
    }

    public void SetBuilding(Building building)
    {
        _building = building;
        _worldTile.SetBuildingSprite(building.Sprite);
    }

   

}
