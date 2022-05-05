using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapStructures : MonoBehaviour
{
    [SerializeField] private Grid[] structures;
    [SerializeField] private Grid mainGrid;

    private MapGenerator mapGenerator;

    private void Start()
    {
        mapGenerator = this.GetComponent<MapGenerator>();
        Random.InitState(mapGenerator.seed);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) DrawStructure(Vector2.zero);
    }

    public List<StructureData> GetStructure()
    {
        List<StructureData> _structureData = new List<StructureData>();
        List<Tilemap> _tileMaps = new List<Tilemap>();

        foreach (Transform tilemap in structures[0].transform)
        {
            _tileMaps.Add(tilemap.GetComponent<Tilemap>());
        }

        foreach (Tilemap _tilemap in _tileMaps)
        {
            Debug.Log(_tilemap.cellBounds);
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

    private void DrawStructure(Vector2 _position)
    {
        List<StructureData> _structureTileInfos = GetStructure();

        foreach (StructureData _struct in _structureTileInfos)
        {
            if (mainGrid.transform.Find("VegetationTileMap").GetComponent<Tilemap>().HasTile(_struct.position)) mainGrid.transform.Find("VegetationTileMap").GetComponent<Tilemap>().SetTile(_struct.position, null);
            if (mainGrid.transform.Find("WaterTileMap").GetComponent<Tilemap>().HasTile(_struct.position)) mainGrid.transform.Find("WaterTileMap").GetComponent<Tilemap>().SetTile(_struct.position, null);

            Tilemap _actualTilemap = mainGrid.transform.Find(_struct.tileMap.name).GetComponent<Tilemap>();
            _actualTilemap.SetTile(_struct.position, _struct.tile);
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
