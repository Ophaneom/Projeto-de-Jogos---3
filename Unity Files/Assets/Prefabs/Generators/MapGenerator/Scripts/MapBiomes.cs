using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBiomes : MonoBehaviour
{
    public Gradient biomeDivision;
    public float denseForestDensity;
    public float lakesDensity;
    [SerializeField] private Tile lakeGround;

    [Header("General Settings")]
    [SerializeField] private float biomesSize;

    [Header("Forest Settings")]
    [SerializeField] private Tile forestGround;
    [SerializeField] private BiomeAssetsController[] forestAssets;

    [Header("Desert Settings")]
    [SerializeField] private Tile desertGround;
    [SerializeField] private BiomeAssetsController[] desertAssets;

    [Header("Taiga Settings")]
    [SerializeField] private Tile taigaGround;
    [SerializeField] private BiomeAssetsController[] taigaAssets;

    [Header("Deep Forest Settings")]
    [SerializeField] private Tile deepForestGround;
    [SerializeField] private BiomeAssetsController[] deepForest_ForestAssets;
    [SerializeField] private BiomeAssetsController[] deepForest_DesertAssets;
    [SerializeField] private BiomeAssetsController[] deepForest_TaigaAssets;

    public Tile CreateTile(Color _tileColor, Sprite _tileSprite, string _tileName)
    {
        Tile _test = ScriptableObject.CreateInstance<Tile>();
        _test.sprite = _tileSprite;
        _test.name = _tileName;
        _test.color = _tileColor;

        return _test;
    }

    public TileBase GetBiome(Vector2 _pos, int _seed, bool _vegetation, bool _ground)
    {
        //GET PERLIN VALUES
        float _mainBodyPerlin = Mathf.PerlinNoise((_pos.x + _seed) * 0.005f, (_pos.y + _seed) * 0.005f);
        float _mainBodyConvertedPerlin = 0;

        float _forestBodyPerlin = Mathf.PerlinNoise((_pos.x + _seed + 300) * 0.005f, (_pos.y + _seed + 300) * 0.005f);

        float _lakesBodyPerlin = Mathf.PerlinNoise((_pos.x + _seed + 150) * 0.005f, (_pos.y + _seed + 150) * 0.005f);

        if (_mainBodyPerlin <= biomeDivision.colorKeys[1].time) _mainBodyConvertedPerlin = 0f;
        else if (_mainBodyPerlin >= biomeDivision.colorKeys[2].time && _mainBodyPerlin <= biomeDivision.colorKeys[3].time) _mainBodyConvertedPerlin = .5f;
        else if (_mainBodyPerlin >= biomeDivision.colorKeys[4].time) _mainBodyConvertedPerlin = 1f;

        else if (_mainBodyPerlin > biomeDivision.colorKeys[1].time && _mainBodyPerlin < biomeDivision.colorKeys[2].time) _mainBodyConvertedPerlin = .25f;
        else if (_mainBodyPerlin > biomeDivision.colorKeys[3].time && _mainBodyPerlin < biomeDivision.colorKeys[4].time) _mainBodyConvertedPerlin = .75f;

        //CREATE AN EMPTY TILE
        Tile _createdTile = null;
        BiomeAssetsController[] _biomeAssets = new BiomeAssetsController[0];

        //SET MAIN GROUND
        if (_mainBodyConvertedPerlin == 0f)
        {
            _createdTile = CreateTile(biomeDivision.Evaluate(0f), desertGround.sprite, "DesertGround");
            _biomeAssets = desertAssets;
        }
        else if (_mainBodyConvertedPerlin == .5f)
        {
            _createdTile = CreateTile(biomeDivision.Evaluate(.5f), forestGround.sprite, "ForestGround");
            _biomeAssets = forestAssets;
        }
        else if (_mainBodyConvertedPerlin == 1f)
        {
            _createdTile = CreateTile(biomeDivision.Evaluate(1f), taigaGround.sprite, "TaigaGround");
            _biomeAssets = taigaAssets;
        }

        else if (_mainBodyConvertedPerlin == .25f)
        {
            _createdTile = CreateTile(biomeDivision.Evaluate(_mainBodyPerlin), forestGround.sprite, "ForestGround");
            _biomeAssets = forestAssets;
        }
        else if (_mainBodyConvertedPerlin == .75f)
        {
            _createdTile = CreateTile(biomeDivision.Evaluate(_mainBodyPerlin), forestGround.sprite, "ForestGround");
            _biomeAssets = forestAssets;
        }

        //SET DENSE FORESTS
        if (_forestBodyPerlin <= denseForestDensity)
        {
            if (_createdTile.name == "ForestGround")
            {
                _createdTile = CreateTile(biomeDivision.Evaluate(_mainBodyPerlin), deepForestGround.sprite, "DeepForest_ForestGround");
                _biomeAssets = deepForest_ForestAssets;
            }
            else if (_createdTile.name == "DesertGround")
            {
                _createdTile = CreateTile(biomeDivision.Evaluate(_mainBodyPerlin), deepForestGround.sprite, "DeepForest_DesertGround");
                _biomeAssets = deepForest_DesertAssets;
            }
            else if (_createdTile.name == "TaigaGround")
            {
                _createdTile = CreateTile(biomeDivision.Evaluate(_mainBodyPerlin), deepForestGround.sprite, "DeepForest_TaigaGround");
                _biomeAssets = deepForest_TaigaAssets;
            }
        }

        //SET LAKES
        if (_lakesBodyPerlin <= lakesDensity)
        {
            _createdTile = CreateTile(Color.white, lakeGround.sprite, "LakeGround");
        }

        //SET VEGETATION AND GROUND
        if (_vegetation)
        {
            foreach (BiomeAssetsController _asset in _biomeAssets)
            {
                float _random = Random.Range(0f, 100f);

                if (_random <= _asset.chance) return _asset.tile;
            }
        }
        else
        {
            return _createdTile;
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
