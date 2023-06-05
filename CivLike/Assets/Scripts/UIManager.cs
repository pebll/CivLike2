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
    private bool tileYieldHidden = true;

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

        // TODO: Call in generate map
        Invoke("CreateTileResourceDisplays",0.1f);
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

    private void CreateTileResourceDisplays()
    {
        for(int x = 0; x < TilemapManager.Instance.MapSize.width; x++)
        {
            for(int y = 0; y < TilemapManager.Instance.MapSize.height; y++)
            {
                GameTile tile = TilemapManager.Instance.GetTileFromPos(new Vector3Int(x,y,0));
          
                if (tile != null)
                {
                    _resourceDisplay.AddDisplayPanel(tile);
                }
            }
        }
        HideTileResourceDisplays();
    }

    private void HideTileResourceDisplays()
    {
        foreach(string id in _resourceDisplay.GetAllIDsOfType(ResourceDisplay.TILE_ID)){
            _resourceDisplay.HideDisplayPanel(id);
        }
    }

    private void ShowTileResourceDisplays()
    {
        foreach (string id in _resourceDisplay.GetAllIDsOfType(ResourceDisplay.TILE_ID))
        {
            _resourceDisplay.ShowDisplayPanel(id);
        }
    }

    private void ToggleTileResourceDisplays()
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
}
