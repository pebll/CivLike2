using Assets._Scripts.Logic.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{
    private BuildingSO _buildingSO;
    private GameTile _position;
    private string _name;
    private Dictionary<ResourceManager.Resource, int> _production;
    private Kingdom _kingdom; 

    // list of buildable tiles

    public BuildingSO BuildingSO => _buildingSO;
    public Dictionary<ResourceManager.Resource, int> Production => _production;
    

    

    public Building(Kingdom kingdom, GameTile position, BuildingSO buildingSO)
    {
        _buildingSO = buildingSO;
        _position = position;
        _kingdom = kingdom;
        _name= buildingSO.GetName();
        _production = ResourceManager.Instance.GetResourceDict(buildingSO.Production);


        position.SetBuilding(this);      
    }
}
