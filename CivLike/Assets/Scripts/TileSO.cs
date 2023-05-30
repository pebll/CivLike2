using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tile")]
public class TileSO : ScriptableObject
{
    public Tile Tile;
    public string Description;
    public int Yield;

}
