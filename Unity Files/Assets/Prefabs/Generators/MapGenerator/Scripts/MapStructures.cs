using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapStructures : MonoBehaviour
{
    [SerializeField] private Grid[] structures;
    [SerializeField] private Grid mainGrid;

    private List<StructureData> GetStructure(int _type)
    {
        List<StructureData> _structureData = new List<StructureData>();
        List<Tilemap> _tileMaps = new List<Tilemap>();

        foreach (Transform tilemap in structures[_type].transform)
        {
            _tileMaps.Add(tilemap.GetComponent<Tilemap>());
        }

        foreach (Tilemap _tilemap in _tileMaps)
        {
            foreach (var _position in _tilemap.cellBounds.allPositionsWithin)
            {
                if (!_tilemap.HasTile(_position)) continue;
                else
                {
                    TileBase _tile = _tilemap.GetTile(_position);
                    _structureData.Add(new StructureData(_tile, _tilemap, _position));
                }
            }
        }
        
        return _structureData;
    }

    public void DrawStructure(Vector2 _position, int _type)
    {
        List<StructureData> _structureTileInfos = GetStructure(_type);

        foreach (StructureData _struct in _structureTileInfos)
        {
            Vector3Int _convertedPos = Vector3Int.zero;
            _convertedPos.x = (int)_position.x + _struct.position.x;
            _convertedPos.y = (int)_position.y + _struct.position.y;

            if (mainGrid.transform.Find("VegetationTileMap").GetComponent<Tilemap>().HasTile(_convertedPos)) mainGrid.transform.Find("VegetationTileMap").GetComponent<Tilemap>().SetTile(_convertedPos, null);
            if (mainGrid.transform.Find("WaterTileMap").GetComponent<Tilemap>().HasTile(_convertedPos)) mainGrid.transform.Find("WaterTileMap").GetComponent<Tilemap>().SetTile(_convertedPos, null);

            Tilemap _actualTilemap = mainGrid.transform.Find(_struct.tileMap.name).GetComponent<Tilemap>();
            _actualTilemap.SetTile(_convertedPos, _struct.tile);
        }
    }
}

public class StructureData
{
    public TileBase tile;
    public Tilemap tileMap;
    public Vector3Int position;

    public StructureData(TileBase _tile, Tilemap _tileMap, Vector3Int _position)
    {
        tile = _tile;
        tileMap = _tileMap;
        position = _position;
    }
}
