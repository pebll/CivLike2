using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building
{ 
    private GameTile _position;
    private string _name;
    private string _description;
    private Kingdom _kingdom; 
    private Sprite _sprite;
    // list of buildable tiles
    public Sprite Sprite => _sprite;
    

    

    public Building(Kingdom kingdom, GameTile position, BuildingSO buildingSO)
    {
        _position = position;
        _kingdom = kingdom;
        _name= buildingSO.GetName();
        _sprite = buildingSO.Sprite;
        _description= buildingSO.Description;

        position.SetBuilding(this);
        // Set new Tile Yield
    }
}
