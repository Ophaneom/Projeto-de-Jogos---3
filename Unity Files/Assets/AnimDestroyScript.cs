using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AnimDestroyScript : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(this.transform.parent.gameObject);
        GameObject.Find("Grid/VegetationTileMap").GetComponent<Tilemap>().SetTile(this.transform.parent.GetComponent<TileInternalData>().pos, null);
    }
}
