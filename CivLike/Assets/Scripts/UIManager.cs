using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject _selectedTileImg;
    [SerializeField] private ResourceDisplay _resourceDisplay;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {


    }
    private void Update()
    {

        UpdateTileResourceDisplay();
    }

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
}
