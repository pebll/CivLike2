using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    private List<Building> _buildings = new List<Building>();


    private void Awake()
    {
        Instance = this;
    }

    public void AddBuilding(Kingdom kingdom, GameTile tile, BuildingSO buildingSO)
    {
        if(!CanBuild(tile, buildingSO)) { return; }
        Building building = new Building(kingdom, tile, buildingSO);
        _buildings.Add(building);
    }

    public void RemoveBuilding(GameTile tile)
    {
        {
        Building building = tile.Building;
        if (building == null)
            {
            return;
        }
        _buildings.Remove(building);
        tile.RemoveBuilding();
        }
    }

    public bool CanBuild(GameTile tile, BuildingSO buildingSO)
    {
        if (tile.Building != null)
        {
            return false;
        }

        foreach (Constants.TileType tileType in buildingSO.MustHaveTileTypes)
        {
            if(!tile.TileSO.TileTypes.Contains(tileType))
            {
                return false;
            }
        }

        foreach (Constants.TileType tileType in buildingSO.CannotHaveTileTypes)
        {
            if (tile.TileSO.TileTypes.Contains(tileType))
            {
                return false;
            }
        }

        return true;
    }
}
