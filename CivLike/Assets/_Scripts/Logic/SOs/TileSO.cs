using Assets._Scripts.Logic.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tile")]
public class TileSO : ScriptableObject
{
    public Tile Tile;
    public string Description;
    public string DisplayName;
    public List<Constants.TileType> TileTypes;
    public List<ResourceManager.Resource> Yield;
}
