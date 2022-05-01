using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    public bool editMode;
    public TileBase tile;

    [SerializeField] private Tilemap constructionMap;
    [SerializeField] private Tilemap vegetationMap;
    private MapDatabase db;

    private void Awake()
    {
        db = this.GetComponent<MapDatabase>();
    }

    private void Update()
    {
        if (!editMode) return;

        Vector3 _point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int _selectedTile = constructionMap.WorldToCell(_point);
            constructionMap.SetTile(_selectedTile, tile);
        }
        if (Input.GetMouseButtonDown(1))
        {
            DestroyTile(_point);
        }
    }

    private void DestroyTile(Vector3 _point)
    {
        Vector3Int _selectedTile = vegetationMap.WorldToCell(_point);
        vegetationMap.SetTile(_selectedTile, null);
        db.StoreData(_selectedTile, null);
    }
}
