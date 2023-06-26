using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldTile : MonoBehaviour
{    
    private GameTile _gameTile;
    [SerializeField] private SpriteRenderer _building;
    public Transform DisplayResourceParent => transform;

    public void Setup(GameTile tile)
    {
        _gameTile = tile;
        transform.position = TilemapManager.Instance.TileToWorldPos(tile.Position);
        name = TilemapManager.Instance.getTileID(tile);
        SetBuildingSprite(null);
        ResourceDisplay.Instance.AddDisplayPanel(tile);
    }

    public void SetBuildingSprite(Sprite sprite)
    {
        _building.sprite = sprite;
    }
}
