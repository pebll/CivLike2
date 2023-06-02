using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField]private GameObject _selectedTileImg;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        UpdateMouseHover();
    }

    public void UpdateMouseHover()
    {
        // TODO: If not on UI
        GameTile tile = TilemapManager.Instance.MouseHoverTile;
        if (tile != null)
        {
            _selectedTileImg.transform.position = TilemapManager.Instance.TileToScreenPos(tile.Position);
            _selectedTileImg.SetActive(true);
        }
        else
        {
            _selectedTileImg.SetActive(false);
        }

    }

}
