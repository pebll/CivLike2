using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Kingdom
{
    private string _name;
    private Color _color;

    private List<GameTile> _tiles;
    private List<City> _cities = new List<City>();

    public void AddCity(GameTile position, List<GameTile> tiles, string name = null)
    {
        City city = BuildingManager.Instance.AddBuilding<City>(this, position, SOManager.Instance.BuildingDict["City"]);
        _cities.Add(city);
        city.Setup(tiles, name);
    }

}
