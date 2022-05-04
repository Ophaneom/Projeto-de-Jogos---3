using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBiomes : MonoBehaviour
{
    [SerializeField] private float biomesSize;
    public Color test;

    private void Awake()
    {
        
    }

    public int GetBiome(Vector2 _pos, int _seed)
    {
        float _perlin1 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed) * biomesSize, (_pos.y + _seed) * biomesSize));
        float _perlin2 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed + 200) * biomesSize, (_pos.y + _seed + 100) * biomesSize));
        float _perlin3 = Mathf.Round(Mathf.PerlinNoise((_pos.x + _seed - 200) * biomesSize, (_pos.y + _seed - 100) * biomesSize));

        Color _p1 = _perlin1 == 1 ? Color.red : new Color(0, 0, 0, 0);
        Color _p2 = _perlin2 == 1 ? Color.green : new Color(0, 0, 0, 0);
        Color _p3 = _perlin3 == 1 ? Color.blue : new Color(0, 0, 0, 0);

        Color _final = _p1 + _p2 + _p3;

        int _biomeType;
        if (_final == new Color(1, 0, 0, 1)) _biomeType = 0;
        else if (_final == new Color(0, 1, 0, 1)) _biomeType = 1;
        else if (_final == new Color(0, 0, 1, 1)) _biomeType = 2;
        else if (_final == new Color(1, 1, 0, 1)) _biomeType = 3;
        else if (_final == new Color(0, 1, 1, 1)) _biomeType = 4;
        else if (_final == new Color(1, 0, 1, 1)) _biomeType = 5;
        else if (_final == new Color(1, 1, 1, 1)) _biomeType = 6;
        else _biomeType = 7;

        return _biomeType;
    }
}
