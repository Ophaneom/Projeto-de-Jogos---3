using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBiomes : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float biomesSize;

    [Header("Deep Forest Settings")]
    [SerializeField] private float deepForestNoise;
    [SerializeField] private TileBase deepForestTree;
    [SerializeField] private TileBase deepForestGround;

    [Header("Forest Settings")]
    [SerializeField] private float forestNoise;
    [SerializeField] private TileBase forestTree;
    [SerializeField] private TileBase forestGround;

    [Header("Desert Settings")]
    [SerializeField] private float desertNoise;
    [SerializeField] private TileBase desertCactus;
    [SerializeField] private TileBase desertGround;

    [Header("Taiga Settings")]
    [SerializeField] private float taigaNoise;
    [SerializeField] private TileBase taigaTree;
    [SerializeField] private TileBase taigaGround;

    [Header("Common Tiles")]
    [SerializeField] private TileBase tinyGrass;
    [SerializeField] private TileBase mediumRock;

    private float generalPerlin = .5f;

    public TileBase GetBiome(Vector2 _pos, int _seed, bool _vegetation, bool _ground)
    {
        float _perlin1 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed) * biomesSize, (_pos.y + _seed) * biomesSize));
        float _perlin2 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed + 200) * biomesSize, (_pos.y + _seed + 100) * biomesSize));
        float _perlin3 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed - 200) * biomesSize, (_pos.y + _seed - 100) * biomesSize));

        Color _p1 = _perlin1 == 1 ? Color.red : new Color(0, 0, 0, 0);
        Color _p2 = _perlin2 == 1 ? Color.green : new Color(0, 0, 0, 0);
        Color _p3 = _perlin3 == 1 ? Color.blue : new Color(0, 0, 0, 0);

        Color _final = _p1 + _p2 + _p3;


        if (_final == new Color(1, 0, 0, 1))
        {
            if (_ground)
            {
                return deepForestGround;
            }
            else
            {
                float _deepForestNoise = Mathf.PerlinNoise((_pos.x + _seed) * generalPerlin, (_pos.y + _seed) * generalPerlin);
                if (_deepForestNoise >= deepForestNoise) return deepForestTree;
            }
        }
        else if (_final == new Color(0, 1, 0, 1)) 
        {
            if (_ground)
            {
                return forestGround;
            }
            else
            {
                float _forestNoise = Mathf.PerlinNoise((_pos.x + _seed) * generalPerlin, (_pos.y + _seed) * generalPerlin);
                if (_forestNoise >= forestNoise) return forestTree;

                float a = Mathf.PerlinNoise((_pos.x + _seed) * generalPerlin, (_pos.y + _seed) * generalPerlin);
                if (a >= .802f) return mediumRock;
                //else return tinyGrass;
            }
        }
        else if (_final == new Color(0, 0, 1, 1))
        {
            if (_ground)
            {
                return desertGround;
            }
            else
            {
                float _desertNoise = Mathf.PerlinNoise((_pos.x + _seed) * generalPerlin, (_pos.y + _seed) * generalPerlin);
                if (_desertNoise >= desertNoise) return desertCactus;
            }
        }
        else
        {
            if (_ground)
            {
                return taigaGround;
            }
            else
            {
                float _taigaNoise = Mathf.PerlinNoise((_pos.x + _seed) * generalPerlin, (_pos.y + _seed) * generalPerlin);
                if (_taigaNoise >= taigaNoise) return taigaTree;
                //else return tinyGrass;
            }
        }

        return null;
    }
}
