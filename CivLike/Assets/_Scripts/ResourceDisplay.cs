using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    public static ResourceDisplay Instance;

    [SerializeField] private Sprite _water;
    [SerializeField] private Sprite _grass;
    [SerializeField] private Sprite _sand;
    [SerializeField] private Sprite _wood;
    [SerializeField] private Sprite _snow;
    [SerializeField] private Sprite _stone;

    [SerializeField] private GameObject _displayPanel;
    [SerializeField] private Image _resourceImage;
    [SerializeField] private SpriteRenderer _resourceSprite;

    private float _baseWidth = 0f;
    private float _resourceSize = 10f;
    private float _resourceSpacing = 5f;
    private float _worldUIFactor = 0.01f;




    private Dictionary<string, GameObject> _displayPanels = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _sprites= new Dictionary<string, Sprite>();

    private void Awake()
    {
        Instance = this;
        _sprites["Water"] = _water;
        _sprites["Grass"] = _grass;
        _sprites["Sand"] = _sand;
        _sprites["Wood"] = _wood;
        _sprites["Snow"] = _snow;
        _sprites["Stone"] = _stone;
    }

    private void AddDisplayPanel(string id, Vector3 pos, Dictionary<ResourceManager.Resource, int> resources, Transform parentTransform = null, bool worldUI = false)
    {
        Transform parent = parentTransform != null ? parentTransform : this.transform;
        float width = _baseWidth;
        //Create the panel
        GameObject displayPanel = Instantiate(new GameObject(), parent);
        displayPanel.transform.position = pos;
        displayPanel.name = id;
        _displayPanels[id] = displayPanel;
        // construct the panel

        UpdateDisplayPanel(id, resources, worldUI);

    }

    private void UpdateDisplayPanel(string id, Dictionary<ResourceManager.Resource, int> resources, bool worldUI = false)
    {
        GameObject displayPanel = _displayPanels[id];
        // TODO: clear past resources
        int resourceAmount = 0;
        List<SpriteRenderer> resourceSprites = new List<SpriteRenderer>();
        List<Image> resourceImages = new List<Image>();
        foreach (KeyValuePair<ResourceManager.Resource, int> entry in resources)
        {
            ResourceManager.Resource resource = entry.Key;
            int count = entry.Value;
            if (count < 3) // show them individualy
            {
                for (int i = 0; i < count; i++)
                {
                    resourceAmount++;
                    // add a resource image
                    if (worldUI)
                    {
                        SpriteRenderer resourceSprite = Instantiate(_resourceSprite, displayPanel.transform);
                        resourceSprite.sprite = _sprites[resource.ToString()];
                        Vector3 imagePos = new Vector3((resourceAmount * (_resourceSize + _resourceSpacing) - _resourceSpacing) * _worldUIFactor, 0, 0);
                        resourceSprite.transform.localPosition = imagePos;
                        resourceSprites.Add(resourceSprite);
                    }
                    else // Normal UI
                    {
                        Image resourceImage = Instantiate(_resourceImage, displayPanel.transform);
                        resourceImage.sprite = _sprites[resource.ToString()];
                        Vector3 imagePos = new Vector3(resourceAmount * (_resourceSize + _resourceSpacing) - _resourceSpacing, 0, 0);
                        resourceImage.transform.localPosition = imagePos;
                        resourceImages.Add(resourceImage);
                    }
                }
            }
            else
            {
                //stack em
            }
        }
        //shift it into place
        Vector3 shiftAmount = new Vector3(((_baseWidth + resourceAmount * (_resourceSize + _resourceSpacing)) / 2 ), 0, 0);
        foreach (Image resourceImage in resourceImages)
        {
            resourceImage.transform.localPosition -= shiftAmount;
        }
        foreach (SpriteRenderer resourceSprite in resourceSprites)
        {
            resourceSprite.transform.localPosition -= shiftAmount * _worldUIFactor;
        }
    }

    public void HideDisplayPanel(string id)
    {
        _displayPanels[id].SetActive(false);
    }

    public void ShowDisplayPanel(string id)
    {
        _displayPanels[id].SetActive(true);
    }
 
    private void RemoveDisplayPanel(string id)
    {
        DestroyAll(_displayPanels[id].gameObject);
        _displayPanels.Remove(id);
    }
   
    public void AddDisplayPanel(GameTile tile)
    {
        string id = TilemapManager.Instance.getTileID(tile);
        Vector3 pos = TilemapManager.Instance.TileToWorldPos(tile.Position);
        Dictionary<ResourceManager.Resource, int> resources = tile.Yield;
        AddDisplayPanel(id, pos, resources, tile.WorldTile.DisplayResourceParent, true);    
    }

    public void RemoveDisplayPanel(GameTile tile)
    {
        RemoveDisplayPanel(TilemapManager.Instance.getTileID(tile));    
    }

    

    private void DestroyAll(GameObject obj)
    {
        foreach (Transform child in obj.transform)
        {
            DestroyAll(child.gameObject);
        }
        Destroy(obj);
    }

    public List<string> GetAllIDsOfType(string type)
    {
        List<string> displays = new List<string>();
        foreach (KeyValuePair<string, GameObject> entry in _displayPanels)
        {
            if (entry.Key.Contains(type))
            {
                displays.Add(entry.Key);
            }
        }
        return displays;
    }


}
