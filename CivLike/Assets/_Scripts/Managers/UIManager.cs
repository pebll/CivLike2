using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _selectedTileImg;
    [SerializeField] private ResourceDisplay _resourceDisplay;

    private Button _showYieldButton;
    private bool tileYieldHidden = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //setup Buttons
        var root = GetComponent<UIDocument>().rootVisualElement;
        _showYieldButton = root.Q<Button>("show-yields-button");
        _showYieldButton.clicked += ToggleTileResourceDisplays;

    }

    private void Update()
    {

    }

    [Obsolete]
    private void UpdateTileResourceDisplay()
    {
        GameTile tile = TilemapManager.Instance.MouseHoverTile;
        GameTile lastTile = TilemapManager.Instance.LastMouseHoverTile;
        if (tile != lastTile)
        {
            if (lastTile != null) // delete last display
            {
                _resourceDisplay.RemoveDisplayPanel(lastTile);
            }
            if (tile != null) //create new display
            {
                _resourceDisplay.AddDisplayPanel(tile);
            }
        }
    }

    private void HideTileResourceDisplays()
    {
        foreach(string id in _resourceDisplay.GetAllIDsOfType(TilemapManager.TILE_ID)){
            _resourceDisplay.HideDisplayPanel(id);
        }
    }

    private void ShowTileResourceDisplays()
    {
        foreach (string id in _resourceDisplay.GetAllIDsOfType(TilemapManager.TILE_ID))
        {
            _resourceDisplay.ShowDisplayPanel(id);
        }
    }

    public void ToggleTileResourceDisplays()
    {
        if (tileYieldHidden)
        {
            ShowTileResourceDisplays();
        }
        else
        {
            HideTileResourceDisplays();
        }
        tileYieldHidden = !tileYieldHidden;
    }

    internal void UpdateKingdomResourceDisplay(Dictionary<ResourceManager.Resource, int> resources)
    {
        throw new NotImplementedException();
    }
}
