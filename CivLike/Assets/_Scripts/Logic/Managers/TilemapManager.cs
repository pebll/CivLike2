using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapManager : MonoBehaviour
{
    [SerializeField] private GameObject _worldTilePrefab;
    public static TilemapManager Instance;
    [SerializeField]
    private Tilemap _tilemap;
    private GameTile[,] _tiles;
    private int _mapWidth;
    private int _mapHeight;
    public const string TILE_ID = "tile";
    public GameTile[,] Tiles => _tiles;

    public (int width, int height) MapSize => (_mapWidth, _mapHeight);

    private GameTile _mouseHoverTile;
    private GameTile _lastMouseHoverTile;
    public GameTile MouseHoverTile => _mouseHoverTile;
    public GameTile LastMouseHoverTile => _lastMouseHoverTile;

    public GameObject WorldTilePrefab=> _worldTilePrefab;

    private void Awake()
    {
        Instance= this;    
    }

    private void Update()
    {
        UpdateMouseHoverTile();
    }

    public void Setup(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth;   
        _mapHeight = mapHeight;
        _tiles = new GameTile[_mapWidth, _mapHeight];
    }
    public void SetTile(Vector3Int position, Tile tile)
    {
        _tilemap.SetTile(position, tile);
    }

    public void AddTile(GameTile tile)
    {      
        _tiles[tile.Position.x, tile.Position.y] = tile;
    }

    public void Load(GameTile[,] tiles = null)
    {     
        if (tiles == null) { tiles = _tiles;}
        foreach (GameTile tile in tiles)
        {
            if (tile != null)
            {
                Vector3Int tilePosition = new Vector3Int(tile.Position.y, tile.Position.x, 0);
                _tilemap.SetTile(tilePosition, tile.Tile);
            }
        }       
    }

    private void UpdateMouseHoverTile()
    {   
        _lastMouseHoverTile = _mouseHoverTile;
        Vector3Int tilePosition = ScreenToTilePos(Input.mousePosition);
        if (tilePosition.x < 0 || tilePosition.x >= _mapWidth || tilePosition.y < 0 || tilePosition.y >= _mapHeight)
            {
            _mouseHoverTile = null;
        }
        else
            {
            _mouseHoverTile = _tiles[tilePosition.x, tilePosition.y];
        }
    }

    public Vector3Int ScreenToTilePos(Vector3 screenPos)
    {
        screenPos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        Vector3Int tilePos = _tilemap.WorldToCell(worldPos);
        tilePos = new Vector3Int(tilePos.y, tilePos.x,0);
        return tilePos;
    }

    public Vector3 TileToScreenPos(Vector3Int tilePos)
    {
        Vector3 worldPos = TileToWorldPos(tilePos);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        return screenPos;
    }

    public GameTile GetTileFromPos(Vector3Int tilePos)
    {
        if (tilePos.x < 0 || tilePos.x >= _mapWidth || tilePos.y < 0 || tilePos.y >= _mapHeight)
            return null;
        return _tiles[tilePos.x, tilePos.y];
    }

    public Vector3 TileToWorldPos(Vector3Int tilePos)
    {
        tilePos = new Vector3Int(tilePos.y, tilePos.x, 0);
        Vector3 worldPos = _tilemap.CellToWorld(tilePos);
        return worldPos;
    }

    public string getTileID(GameTile tile)
    {
        return TILE_ID + tile.Position.ToString();
    }
}
