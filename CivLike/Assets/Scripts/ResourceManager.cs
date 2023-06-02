using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public enum Resource 
    { Water, Grass, Sand, Snow, Stone, Wood }

    // RESOURCE FORMAT: Dictionary<Resource resource, int amount>
    // Dictionary<Resource, int>

    private Dictionary<Resource, int> GetEmptyResourceDict()
    {
        Dictionary<Resource, int> resourceDict = new Dictionary<Resource, int>();
        foreach (Resource resource in System.Enum.GetValues(typeof(Resource)))
        {
            resourceDict.Add(resource, 0);
        }
        return resourceDict;
    }

    public Dictionary<Resource, int> GetResourceDict(List<Resource> resources)
    {
        Dictionary<Resource, int> resourceDict = GetEmptyResourceDict();
        foreach (Resource resource in resources)
        {
            resourceDict[resource] += 1;
        }
        return resourceDict;
    }

}
