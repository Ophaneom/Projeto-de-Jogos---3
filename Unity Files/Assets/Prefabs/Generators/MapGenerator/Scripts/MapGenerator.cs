using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

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
    [SerializeField] private Tilemap water;
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase tinyGrass;
    [SerializeField] private TileBase tallGrass;
    [SerializeField] private TileBase tree;

    public Vector2 centerSpacement;
    private MapRenderer mapRenderer;
    private MapDatabase mapDatabase;

    private void Awake()
    {
        centerSpacement.x = chunkSize.x % 2 == 0 ? 0 : .5f;
        centerSpacement.y = chunkSize.y % 2 == 0 ? 0 : .5f;

        mapRenderer = this.GetComponent<MapRenderer>();
        chunkSize = mapRenderer.chunkSize;

        mapDatabase = this.GetComponent<MapDatabase>();
    }

    public void DrawChunk(List<Vector2> _chunksToRender)
    {
        foreach(Vector2 _chunk in _chunksToRender)
        {
            Vector2 _startPos = new Vector2(_chunk.x - ((chunkSize.x / 2) - centerSpacement.x),
            _chunk.y - ((chunkSize.y / 2) - centerSpacement.y));

            //Generate tiles per perlin position
            for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
            {
                for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
                {
                    float _groundPerlin = Mathf.PerlinNoise((_x + seed) * groundPerlinSize, (_y + seed) * groundPerlinSize);
                    if (_groundPerlin > .5f)
                    {
                        ground.SetTile(new Vector3Int(_x, _y), groundTile);

                        var _tile = mapDatabase.tiles.FirstOrDefault(o => o.tilePosition == new Vector3Int(_x, _y));
                        if (_tile != null)
                        {
                            vegetation.SetTile(new Vector3Int(_x, _y), _tile.tileReference);
                        }
                        else
                        {
                            float _vegetationPerlin = Mathf.PerlinNoise((_x + 512 + seed) * vegetationPerlinSize, (_y + 512 + seed) * vegetationPerlinSize);
                            //if (_vegetationPerlin >= .3f && _groundPerlin < .6f) vegetation.SetTile(new Vector3Int(_x, _y), tinyGrass);
                            if (_vegetationPerlin >= .8f) vegetation.SetTile(new Vector3Int(_x, _y), tallGrass);

                            float _treePerlin = Mathf.PerlinNoise((_x + 128 + seed) * treePerlinSize, (_y + 128 + seed) * treePerlinSize);
                            if (_treePerlin > .8f) vegetation.SetTile(new Vector3Int(_x, _y), tree);
                        }
                    }
                    else water.SetTile(new Vector3Int(_x, _y), waterTile);
                }
            }
        }
    }

    public void RemoveChunks(List<Vector2> _chunksToRemove)
    {
        foreach(Vector2 _chunk in _chunksToRemove)
        {
            Vector2 _startPos = new Vector2(_chunk.x - ((chunkSize.x / 2) - centerSpacement.x),
            _chunk.y - ((chunkSize.y / 2) - centerSpacement.y));

            for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
            {
                for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
                {
                    ground.SetTile(new Vector3Int(_x, _y), null);
                    water.SetTile(new Vector3Int(_x, _y), null);
                    vegetation.SetTile(new Vector3Int(_x, _y), null);
                }
            }
        }
    }
}
