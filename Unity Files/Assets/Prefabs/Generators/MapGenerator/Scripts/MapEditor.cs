using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapEditor : MonoBehaviour
{
    public GameObject grid;

    public TileBase tileObject;
    public Material shader;
    public bool placing;
    public Color canPlaceColor, cannotPlaceColor;

    private Tilemap tilemp;
    private Tilemap prevTileMap;
    private Vector3Int oldPos;

    void Start()
    {
        tilemp = GameObject.Find("VegetationTileMap").GetComponent<Tilemap>();
        prevTileMap = GameObject.Find("ConstructionTileMap").GetComponent<Tilemap>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) placing = true;
        if (placing && Input.GetKeyDown(KeyCode.Escape))
        {
            prevTileMap.SetTile(oldPos, null);
            placing = false;
            grid.SetActive(false);
        }

        if (placing)
        {
            grid.SetActive(true);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemp.WorldToCell(mousePos);

            TilemapRenderer a = GameObject.Find("ConstructionTileMap").GetComponent<TilemapRenderer>();

            if (!tilemp.HasTile(gridPos))
            {
                
                if (gridPos != oldPos)
                {
                    prevTileMap.SetTile(gridPos, tileObject);
                    prevTileMap.SetTile(oldPos, null);

                    GameObject _gameObject = prevTileMap.GetInstantiatedObject(gridPos);

                    _gameObject.transform.Find("Physic_Collider").GetComponent<BoxCollider2D>().enabled = false;
                    _gameObject.transform.Find("Touch_Collider").GetComponent<BoxCollider2D>().enabled = false;

                    _gameObject.transform.Find("Skin").GetComponent<SpriteRenderer>().material = shader;
                    shader.SetColor("_ColorPlacement", canPlaceColor);
                    _gameObject.transform.Find("Skin").GetComponent<SpriteRenderer>().sortingLayerName = "Foreground Effects";

                    oldPos = gridPos;
                }
            }
            else
            {
                if (gridPos != oldPos)
                {
                    prevTileMap.SetTile(gridPos, tileObject);
                    prevTileMap.SetTile(oldPos, null);

                    GameObject _gameObject = prevTileMap.GetInstantiatedObject(gridPos);

                    _gameObject.transform.Find("Physic_Collider").GetComponent<BoxCollider2D>().enabled = false;
                    _gameObject.transform.Find("Touch_Collider").GetComponent<BoxCollider2D>().enabled = false;

                    _gameObject.transform.Find("Skin").GetComponent<SpriteRenderer>().material = shader;
                    shader.SetColor("_ColorPlacement", cannotPlaceColor);
                    _gameObject.transform.Find("Skin").GetComponent<SpriteRenderer>().sortingLayerName = "Foreground Effects";

                    oldPos = gridPos;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (!tilemp.HasTile(gridPos))
                {
                    prevTileMap.SetTile(oldPos, null);
                    tilemp.SetTile(gridPos, tileObject);
                    placing = false;
                    grid.SetActive(false);
                }
            }
        }
    }
}
