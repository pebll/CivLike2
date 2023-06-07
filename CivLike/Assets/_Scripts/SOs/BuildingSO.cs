using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class BuildingSO : ScriptableObject
{
    public Sprite Sprite;
    public string Description;
    public string DisplayName;
    public List<ResourceManager.Resource> Production;
    public bool removeTileYield = false;

    public string GetName()
    {
        return DisplayName == null ? name : DisplayName;
    }
}
