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
    private Dictionary<ResourceManager.Resource, int> _resources;

    public Kingdom() {
        _name = "Kingdom";
        _color = Color.red;
        _resources = ResourceManager.Instance.GetEmptyResourceDict();
        _cities = new List<City>();
        UpdateResourceDisplay();
        
    }

    public void AddCity(GameTile position, List<GameTile> tiles, string name = null)
    {
        City city = BuildingManager.Instance.AddBuilding<City>(this, position, SOManager.Instance.BuildingDict["City"]);
        _cities.Add(city);
        city.Setup(tiles, name);
    }

    private void AddResources(Dictionary<ResourceManager.Resource, int> resources)
    {
        ResourceManager.Instance.UpdateResourceDict(_resources, addResources: resources);
        UpdateResourceDisplay();
    }

    private void RemoveResources(Dictionary<ResourceManager.Resource, int> resources)
    {
        ResourceManager.Instance.UpdateResourceDict(_resources, removeResources: resources);
        UpdateResourceDisplay();
    }

    private void UpdateResourceDisplay()
    {
        UIManager.Instance.UpdateKingdomResourceDisplay(_resources);
    }

}
