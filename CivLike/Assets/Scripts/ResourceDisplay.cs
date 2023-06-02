using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Sprite _water;
    [SerializeField] private Sprite _grass;
    [SerializeField] private Sprite _sand;
    [SerializeField] private Sprite _wood;
    [SerializeField] private Sprite _snow;
    [SerializeField] private Sprite _stone;

    [SerializeField] private GameObject _displayPanel;
    [SerializeField] private Image _resourceImage;

    [SerializeField] private float _baseWidth = 0f;
    [SerializeField] private float _resourceSize = 10f;
    [SerializeField] private float _resourceSpacing = 5f;
    

    private Dictionary<string, GameObject> _displayPanels = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _sprites= new Dictionary<string, Sprite>();

    private void Awake()
    {
        _sprites["Water"] = _water;
        _sprites["Grass"] = _grass;
        _sprites["Sand"] = _sand;
        _sprites["Wood"] = _wood;
        _sprites["Snow"] = _snow;
        _sprites["Stone"] = _stone;
    }

    private void AddDisplayPanel(string id, Vector3 pos, Dictionary<ResourceManager.Resource, int> resources)
    {
    
        float width = _baseWidth;
        //Create the panel
        GameObject displayPanel = Instantiate(_displayPanel, this.transform);
        displayPanel.transform.position = pos;
        displayPanel.name = id;
        _displayPanels[id] = displayPanel;
        // construct the panel
        int resourceAmount = 0;
        List<Image> resourceImages = new List<Image>();
        foreach (KeyValuePair<ResourceManager.Resource, int> entry in resources)
        {
            ResourceManager.Resource resource = entry.Key;
            int count = entry.Value;
            if (count < 3) // show them individualy
            {
                for(int i = 0; i< count; i++)
                {
                    resourceAmount++;
                    // add a resource image
                    Image resourceImage = Instantiate(_resourceImage, displayPanel.transform);
                    resourceImage.sprite = _sprites[resource.ToString()];
                    Vector3 imagePos = new Vector3(resourceAmount * (_resourceSize + _resourceSpacing) - _resourceSpacing, 0, 0);
                    resourceImage.transform.localPosition = imagePos;
                    resourceImages.Add(resourceImage);
                }                       
            }
            else
            {
                //stack em
            }
        }
        // size (deactivate) the panel
        //_displayPanel.transform.GetChild(0).localScale = new Vector3(_baseWidth + resourceAmount * (_resourceSize + _resourceSpacing) - _resourceSpacing, 1, 1);
        _displayPanel.transform.GetChild(0).gameObject.SetActive(false);
        //shift it into place
        Vector3 shiftAmount = new Vector3((_baseWidth + resourceAmount * (_resourceSize + _resourceSpacing))/2, 0, 0);
        foreach (Image resourceImage in resourceImages)
        {
            resourceImage.transform.localPosition -= shiftAmount;
        }

    }
 
    private void RemoveDisplayPanel(string id)
    {
        DestroyAll(_displayPanels[id].gameObject);
        _displayPanels.Remove(id);
    }
    public void AddDisplayPanel(GameTile tile)
    {
        string id = getTileID(tile);
        Vector3 pos = TilemapManager.Instance.TileToScreenPos(tile.Position);
        Dictionary<ResourceManager.Resource, int> resources = tile.Yield;
        AddDisplayPanel(id, pos, resources);    
    }

    public void RemoveDisplayPanel(GameTile tile)
    {
        RemoveDisplayPanel(getTileID(tile));    
    }

    public string getTileID(GameTile tile)
    {
        return "tile" + tile.Position.ToString();
    }

    private void DestroyAll(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            DestroyAll(child.gameObject);
        }
        Destroy(obj);
    }


}
