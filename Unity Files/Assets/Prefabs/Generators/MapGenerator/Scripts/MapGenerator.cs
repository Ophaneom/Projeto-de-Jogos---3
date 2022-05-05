using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class MapGenerator : MonoBehaviour
{
    [Header("General Settings")]
    public int seed;

    [Header("Chunks Settings")]
    private Vector2 chunkSize;
    [SerializeField] private float groundPerlinSize;
    [SerializeField] private float vegetationPerlinSize;
    [SerializeField] private float treePerlinSize;

    [Header("Tilemaps Settings")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap vegetation;
    [SerializeField] private Tilemap water;

    [Header("Tiles Settings")]
    [SerializeField] private TileBase groundTile;
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase tinyGrassTile;
    [SerializeField] private TileBase tallGrassTile;
    [SerializeField] private TileBase treeTile;

    [Header("Test")]
    [SerializeField] private TileBase[] tests;

    public Vector2 centerSpacement;
    private MapRenderer mapRenderer;
    private MapDatabase mapDatabase;
    private MapBiomes mapBiomes;

    private void Awake()
    {
        mapDatabase = this.GetComponent<MapDatabase>();
        mapBiomes = this.GetComponent<MapBiomes>();
        mapRenderer = this.GetComponent<MapRenderer>();
        chunkSize = mapRenderer.chunkSize;

        centerSpacement = Vector2.zero;
        //centerSpacement.x = chunkSize.x % 2 == 0 ? 0 : .5f;
        //centerSpacement.y = chunkSize.y % 2 == 0 ? 0 : .5f;
    }

    public void DrawChunk(List<Vector2> _chunksToRender)
    {
        foreach(Vector2 _chunk in _chunksToRender)
        {
            Vector2 _startPos = new Vector2(_chunk.x - ((chunkSize.x / 2) - centerSpacement.x),
            _chunk.y - ((chunkSize.y / 2) - centerSpacement.y));

            for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
            {
                for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
                {
                    float _groundPerlin = Mathf.PerlinNoise((_x + seed) * groundPerlinSize, (_y + seed) * groundPerlinSize);
                    if (_groundPerlin > .5f)
                    {
                        ground.SetTile(new Vector3Int(_x, _y), mapBiomes.GetBiome(new Vector2(_x, _y), seed, false, true));

                        vegetation.SetTile(new Vector3Int(_x, _y), mapBiomes.GetBiome(new Vector2(_x, _y), seed, true, false));
                    }
                    else water.SetTile(new Vector3Int(_x, _y), waterTile);
                }
            }

            //mapDatabase.SaveChunkData(_chunk);
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

    public void AddVegetation(Vector2 _mapPosition)
    {
        float _biomePerlin = Mathf.PerlinNoise((_mapPosition.x + seed) * .05f, (_mapPosition.y + seed) * .05f);

        if (_biomePerlin < .33f)
        {

        }
        else if (_biomePerlin >= .33f && _biomePerlin < .66f)
        {

        }
        else
        {

        }
    }
}
