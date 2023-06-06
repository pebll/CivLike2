using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class City
{
    private List<GameTile> _tiles;
    private GameTile _position;
    private string _name;
    private Kingdom _kingdom;
    private int _population = 1;

    public City(Kingdom kingdom, GameTile position, List<GameTile> tiles, string name = null)
    {
        _name = name == null ? GenerateCityName() : name;
        _position = position;
        _kingdom = kingdom;
        // TODO: Check if given tiles are 2 and in range of 1
        _tiles = tiles;
        _tiles.Add(position);     
    }

   string GenerateCityName()
    {
        //TODO: implement
        return "city";
    }
}
