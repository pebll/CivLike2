using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



public class GameTile
{
    private TileSO _tileSO;
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
        _tileSO= tileSO;
        _tile = tileSO.Tile;
        _description = tileSO.Description;
        _yield = GetRawTileYield();
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
        _worldTile.SetBuildingSprite(building.BuildingSO.Sprite);
        UpdateYield(building.Production, building.BuildingSO.removeTileYield);
    }

    public void RemoveBuilding()
    {
        _building = null;
        _worldTile.SetBuildingSprite(null);
        UpdateYield(GetRawTileYield(), true);
    }

    private Dictionary<ResourceManager.Resource, int> GetRawTileYield()
    {
        return ResourceManager.Instance.GetResourceDict(_tileSO.Yield);
    }

    private void UpdateYield(Dictionary<ResourceManager.Resource, int> addYield, bool resetBeforeAdding = false)
    {
        // change yield dict
        if (resetBeforeAdding)
        {
            _yield = ResourceManager.Instance.GetEmptyResourceDict();
        }
        foreach (KeyValuePair<ResourceManager.Resource, int> entry in addYield)
        {
            _yield[entry.Key] += entry.Value;
        }
        // update display
        ResourceDisplay.Instance.UpdateDisplayPanel(this);

    }
   

}
