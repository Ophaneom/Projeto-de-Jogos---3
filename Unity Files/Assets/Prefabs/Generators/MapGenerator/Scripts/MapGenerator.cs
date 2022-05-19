using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Header("General Settings")]
    public int seed;

    [Header("Chunks Settings")]
    private Vector2 chunkSize;
    [SerializeField] private float groundPerlinSize;

    [Header("Tilemaps Settings")]
    [SerializeField] private Tilemap ground;
    [SerializeField] private Tilemap vegetation;
    [SerializeField] private Tilemap water;
    [SerializeField] private Tilemap grid;

    [Header("Tiles Settings")]
    [SerializeField] private TileBase waterTile;
    [SerializeField] private TileBase gridTile;

    private MapRenderer mapRenderer;
    private MapDatabase mapDatabase;
    private MapBiomes mapBiomes;
    private MapStructures mapStructures;

    private void Awake()
    {
        Random.InitState(seed);

        mapDatabase = this.GetComponent<MapDatabase>();
        mapBiomes = this.GetComponent<MapBiomes>();
        mapStructures = this.GetComponent<MapStructures>();
        mapRenderer = this.GetComponent<MapRenderer>();
        chunkSize = mapRenderer.chunkSize;
    }

    public void DrawChunk(List<Vector2> _chunksToRender)
    {
        foreach(Vector2 _chunk in _chunksToRender)
        {
            ChunkData _chunkData = mapDatabase.storedChunks.Find(item => item.chunk == _chunk);

            if (_chunkData != null)
            {
                foreach(TileData _tileData in _chunkData.tiles)
                {
                    _tileData.tileMap.SetTile(_tileData.position, _tileData.tile);
                }
            }
            else
            {
            Vector2 _startPos = new Vector2(_chunk.x - (chunkSize.x / 2),
            _chunk.y - (chunkSize.y / 2));

                for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
                {
                    for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
                    {
                        if (!ground.HasTile(new Vector3Int(_x, _y)))
                        {
                            TileBase _groundTile = mapBiomes.GetBiome(new Vector2(_x, _y), seed, false, true);
                            grid.SetTile(new Vector3Int(_x, _y), gridTile);

                            if (_groundTile.name == "LakeGround") water.SetTile(new Vector3Int(_x, _y), _groundTile);
                            else
                            {
                                ground.SetTile(new Vector3Int(_x, _y), _groundTile);

                                TileBase _vegetationTile = mapBiomes.GetBiome(new Vector2(_x, _y), seed, true, false);
                                vegetation.SetTile(new Vector3Int(_x, _y), _vegetationTile);

                                if (_vegetationTile != null)
                                {
                                    GameObject _gameObject = vegetation.GetInstantiatedObject(new Vector3Int(_x, _y));
                                    if (_gameObject != null)
                                    {
                                        if (_gameObject.GetComponent<TileInternalData>())
                                        {
                                            _gameObject.GetComponent<TileInternalData>().pos = new Vector3Int(_x, _y);
                                        }
                                    }
                                }
                            }

                            if (Random.Range(0f, 100f) <= .001f) mapStructures.DrawStructure(new Vector2(_x, _y), 0);
                        }
                    }
                }

                mapDatabase.SaveChunkData(_chunk);
            }
        }
    }

    public void RemoveChunks(List<Vector2> _chunksToRemove)
    {
        foreach(Vector2 _chunk in _chunksToRemove)
        {
            mapDatabase.SaveChunkData(_chunk);

            Vector2 _startPos = new Vector2(_chunk.x - (chunkSize.x / 2),
            _chunk.y - (chunkSize.y / 2));

            for (var _y = (int)_startPos.y; _y < (int)_startPos.y + chunkSize.y; _y++)
            {
                for (var _x = (int)_startPos.x; _x < (int)_startPos.x + chunkSize.x; _x++)
                {
                    ground.SetTile(new Vector3Int(_x, _y), null);
                    water.SetTile(new Vector3Int(_x, _y), null);
                    vegetation.SetTile(new Vector3Int(_x, _y), null);
                    grid.SetTile(new Vector3Int(_x, _y), null);
                }
            }
        }
    }
}
