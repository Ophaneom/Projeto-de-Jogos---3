using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    Tilemap tilemp;
    public TileBase test;

    void Start()
    {
        tilemp = GameObject.Find("VegetationTileMap").GetComponent<Tilemap>();
    }
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int gridPos = tilemp.WorldToCell(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            if (tilemp.HasTile(gridPos))
            {
                //tilemp.SetTile(gridPos, null);
            }
        }

        else if (Input.GetMouseButtonDown(1))
        {
            //tilemp.SetTile(gridPos, test);
        }
    }
}
