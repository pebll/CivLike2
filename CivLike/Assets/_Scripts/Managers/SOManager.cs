using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOManager : MonoBehaviour
{
    public static SOManager Instance;
    private Dictionary<string, TileSO> _tileDict = new Dictionary<string, TileSO>();
    private Dictionary<string, BuildingSO> _buildingDict = new Dictionary<string, BuildingSO>();
    public Dictionary<string, BuildingSO> BuildingDict => _buildingDict;
    public Dictionary<string, TileSO> TileDict => _tileDict;



    private void Awake()
    {
        Instance = this;
        TileSO[] tileArray = Resources.LoadAll<TileSO>("Tiles");
        foreach (TileSO tileSO in tileArray)
        {
            _tileDict.Add(tileSO.name, tileSO);
        }
        BuildingSO[] buildingArray = Resources.LoadAll<BuildingSO>("Buildings");
        foreach (BuildingSO buildingSO in buildingArray)
        {
            _buildingDict.Add(buildingSO.name, buildingSO);
        }
    }
}
