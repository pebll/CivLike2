using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class City:Building
{
    private List<GameTile> _tiles = new List<GameTile>();
    private GameTile _position;
    private string _name;
    private Kingdom _kingdom;
    private int _population = 1;

    public City(Kingdom kingdom, GameTile position, BuildingSO buildingSO = null)
        :base(kingdom, position, SOManager.Instance.BuildingDict["City"]) { }


    public void Setup(List<GameTile> tiles, string name)
    {
        _name = name == null ? GenerateCityName() : name;
        // TODO: Check if given tiles are 2 and in range of 1
        _tiles.AddRange(tiles);
        _tiles.Add(_position);
    }

   string GenerateCityName()
    {
        //TODO: implement
        return "city";
    }
}
