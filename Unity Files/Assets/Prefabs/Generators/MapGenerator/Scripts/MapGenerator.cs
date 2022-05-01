using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("General Settings")]
    public int seed;

    [Header("Chunk Settings")]
    private Vector2 chunkSize;
    [SerializeField] private float groundPerlinSize;
    [SerializeField] private float vegetationPerlinSize;
    [SerializeField] private float treePerlinSize;

    [Header("Tile Settings")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap vegetation;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase tinyGrass;
    [SerializeField] private TileBase tallGrass;
    [SerializeField] private TileBase tree;

    public Vector2 centerSpacement;
    private MapRenderer mapRenderer;

    private void Awake()
    {
        centerSpacement.x = chunkSize.x % 2 == 0 ? 0 : .5f;
        centerSpacement.y = chunkSize.y % 2 == 0 ? 0 : .5f;
        mapRenderer = this.GetComponent<MapRenderer>();
        chunkSize = mapRenderer.chunkSize;
    }

    public void DrawChunk(Vector2 _centerPos)
    {
        Vector2 _startPos = new Vector2 (_centerPos.x - ((chunkSize.x / 2) - centerSpacement.x),
            _centerPos.y - ((chunkSize.y / 2) - centerSpacement.y));
        
        //Generate tiles per perlin position
        for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
        {
            for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
            {
                float _groundPerlin = Mathf.PerlinNoise((_x + seed) * groundPerlinSize, (_y + seed) * groundPerlinSize);
                if (_groundPerlin > .5f)
                {
                    ground.SetTile(new Vector3Int(_x, _y), groundTile);

                    float _vegetationPerlin = Mathf.PerlinNoise((_x + 512 + seed) * vegetationPerlinSize, (_y + 512 + seed) * vegetationPerlinSize);
                    if (_vegetationPerlin >= .3f && _groundPerlin < .6f) vegetation.SetTile(new Vector3Int(_x, _y), tinyGrass);
                    else if (_vegetationPerlin >= .6f && _groundPerlin < .8f) vegetation.SetTile(new Vector3Int(_x, _y), tallGrass);

                    float _treePerlin = Mathf.PerlinNoise((_x + 128 + seed) * treePerlinSize, (_y + 128 + seed) * treePerlinSize);
                    if (_treePerlin > .8f) vegetation.SetTile(new Vector3Int(_x, _y), tree);
                }
            }
        }
    }

    public void ResetChunks()
    {
        ground.ClearAllTiles();
        vegetation.ClearAllTiles();
    }
}
