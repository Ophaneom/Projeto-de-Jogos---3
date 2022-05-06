using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDatabase : MonoBehaviour
{
    public List<ChunkData> storedChunks = new List<ChunkData>();

    [SerializeField] private Tilemap[] tileMaps;

    private MapRenderer mapRenderer;
    private Vector2 centerSpacement;
    private Vector2 chunkSize;

    private void Awake()
    {
        mapRenderer = this.GetComponent<MapRenderer>();
        chunkSize = mapRenderer.chunkSize;

        centerSpacement = Vector2.zero;
    }

    public void SaveChunkData(Vector2 _chunk)
    {
        ChunkData item = storedChunks.Find(item => item.chunk == _chunk);
        if (item == null)
        {
            storedChunks.Add(new ChunkData(_chunk, chunkSize, centerSpacement, tileMaps));
        }
        else
        {
            storedChunks.Remove(item);
            storedChunks.Add(new ChunkData(_chunk, chunkSize, centerSpacement, tileMaps));
        }
    }
}

public class ChunkData 
{
    public Vector2 chunk;
    public List<TileData> tiles = new List<TileData>();

    public ChunkData(Vector2 _chunk, Vector2 _chunkSize, Vector2 _centerSpacement, Tilemap[] _tileMaps)
    {
        chunk = _chunk;

        Vector2 _startPos = new Vector2(_chunk.x - ((_chunkSize.x / 2) - _centerSpacement.x),
            _chunk.y - ((_chunkSize.y / 2) - _centerSpacement.y));

        for (var _y = (int)_startPos.y; _y < (int)_startPos.y + _chunkSize.y; _y++)
        {
            for (var _x = (int)_startPos.x; _x < (int)_startPos.x + _chunkSize.x; _x++)
            {
                foreach (Tilemap _tileMap in _tileMaps)
                {
                    tiles.Add(new TileData(_tileMap, _tileMap.GetTile(new Vector3Int(_x, _y)), new Vector3Int(_x, _y)));
                }
            }
        }
    }
}

public class TileData
{
    public Tilemap tileMap;
    public TileBase tile;
    public Vector3Int position;

    public TileData(Tilemap _tileMap, TileBase _tile, Vector3Int _position)
    {
        tileMap = _tileMap;
        tile = _tile;
        position = _position;
    }
}



