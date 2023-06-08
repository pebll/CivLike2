using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Test", 0.1f);
    }

    private void Test()
    {
        GameTile tile = TilemapManager.Instance.GetTileFromPos(new Vector3Int(5, 5, 0));
        City city = new City(new Kingdom(), tile, new List<GameTile>(), null);
    }
   
}
