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
    [SerializeField]
    public List<ResourceManager.Resource> Yield;
    //[System.NonSerialized]
    //public Dictionary<ResourceManager.Resource, int> Yield;

    //private void OnEnable()
    //{
    //    Yield = ResourceManager.Instance.GetResourceDict(_yield);
    //}


}
