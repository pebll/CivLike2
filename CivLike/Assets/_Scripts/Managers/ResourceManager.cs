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
    { Water, Grass, Sand, Snow, Stone, Wood, Joker }

    // RESOURCE FORMAT: Dictionary<Resource resource, int amount>
    // Dictionary<Resource, int>

    public Dictionary<Resource, int> GetEmptyResourceDict()
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
    public Dictionary<Resource, int> UpdateResourceDict(
        Dictionary<Resource, int> originalResourceDict,
        Dictionary<Resource, int> removeResources = null, 
        Dictionary<Resource, int> addResources = null,
        bool resetBeforeAdding = false)
    {
        if (resetBeforeAdding)
        {
            originalResourceDict = GetEmptyResourceDict();
        }
        if(addResources != null)
        {
            foreach (KeyValuePair<Resource, int> entry in addResources)
                {
                    originalResourceDict[entry.Key] += entry.Value;
            }        
        }
        if (removeResources != null)
        {
            foreach (KeyValuePair<Resource, int> entry in removeResources)
            {
                originalResourceDict[entry.Key] -= entry.Value;
            }
        }      
        return originalResourceDict;

    }

}
