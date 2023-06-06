using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldTile : MonoBehaviour
{    
    private GameTile _gameTile;
    [SerializeField] private Transform _displayResourceParent;
    public Transform DisplayResourceParent => _displayResourceParent;

    public void Setup(GameTile tile)
    {
        _gameTile = tile;
        transform.position = TilemapManager.Instance.TileToWorldPos(tile.Position);
        name = TilemapManager.Instance.getTileID(tile);

        ResourceDisplay.Instance.AddDisplayPanel(tile);
    }
}
