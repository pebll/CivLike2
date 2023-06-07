using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Kingdom
{
    private string _name;
    private Color _color;

    private List<GameTile> _tiles;
    private List<City> _cities;

    private void AddCity(GameTile position, List<GameTile> tiles, string name = null)
    {
        City city = new City(this, position, tiles, name);
        _cities.Add(city);
    }

}
