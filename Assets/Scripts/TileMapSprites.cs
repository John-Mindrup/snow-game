using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapSprites : MonoBehaviour
{
    private static TileMapSprites _instance;
    public static TileMapSprites Instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public Tile nPrints;
    public Tile ePrints;
    public Tile sPrints;
    public Tile wPrints;
    public Tile tPrints;

}
