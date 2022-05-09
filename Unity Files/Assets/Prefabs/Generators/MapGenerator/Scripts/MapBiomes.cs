using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBiomes : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float biomesSize;

    [Header("Deep Forest Settings")]
    [SerializeField] private TileBase deepForestGround;
    [SerializeField] private BiomeAssetsController[] deepForestAssets;

    [Header("Forest Settings")]
    [SerializeField] private TileBase forestGround;
    [SerializeField] private BiomeAssetsController[] forestAssets;

    [Header("Desert Settings")]
    [SerializeField] private TileBase desertGround;
    [SerializeField] private BiomeAssetsController[] desertAssets;

    [Header("Taiga Settings")]
    [SerializeField] private TileBase taigaGround;
    [SerializeField] private BiomeAssetsController[] taigaAssets;

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
            if (_ground) return deepForestGround;
            else
            {
                foreach(BiomeAssetsController _asset in deepForestAssets)
                {
                    float _random = Random.Range(0f, 100f);

                    if (_random <= _asset.chance) return _asset.tile;
                }
            }
        }
        
        else if (_final == new Color(0, 1, 0, 1)) 
        {
            if (_ground) return taigaGround;
            else
            {
                foreach (BiomeAssetsController _asset in taigaAssets)
                {
                    float _random = Random.Range(0f, 100f);

                    if (_random <= _asset.chance) return _asset.tile;
                }
            }
        }
        
        else if (_final == new Color(0, 0, 1, 1))
        {
            if (_ground) return desertGround;
            else
            {
                foreach (BiomeAssetsController _asset in desertAssets)
                {
                    float _random = Random.Range(0f, 100f);

                    if (_random <= _asset.chance) return _asset.tile;
                }
            }
        }
        
        else
        {
            if (_ground) return forestGround;
            else
            {
                foreach (BiomeAssetsController _asset in forestAssets)
                {
                    float _random = Random.Range(0f, 100f);

                    if (_random <= _asset.chance) return _asset.tile;
                }
            }
        }

        return null;
    }
}

[System.Serializable]
public class BiomeAssetsController
{
    public TileBase tile;
    public float chance;
}
