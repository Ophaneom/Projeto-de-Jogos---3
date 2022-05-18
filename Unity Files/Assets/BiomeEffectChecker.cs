using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class BiomeEffectChecker : MonoBehaviour
{
    public GameObject coldEffect;
    private Tilemap tilemp;
    private GameObject player;

    void Start()
    {
        tilemp = GameObject.Find("GroundTileMap").GetComponent<Tilemap>();
        player = GameObject.Find("Player").gameObject;
    }
    void Update()
    {
        Vector3Int gridPos = tilemp.WorldToCell(player.transform.position);

        if (tilemp.HasTile(gridPos))
        {
            Color coldEffectColor = coldEffect.GetComponent<Image>().color;

            if (tilemp.GetTile(gridPos).name == "TaigaGroundRuleTile")
            {
                if (coldEffect.transform.Find("SnowParticles").GetComponent<ParticleSystem>().emissionRate == 0)
                {
                    coldEffect.transform.Find("SnowParticles").GetComponent<ParticleSystem>().emissionRate = 50;
                }
                

                if (coldEffectColor.a < 1)
                {
                    coldEffect.GetComponent<Image>().color = new Color(coldEffectColor.r, coldEffectColor.g, coldEffectColor.b, coldEffectColor.a += .001f);
                }
                else
                {
                    if (coldEffectColor.a > 1) coldEffect.GetComponent<Image>().color = new Color(coldEffectColor.r, coldEffectColor.g, coldEffectColor.b, 1);
                }
            }
            else
            {
                if (coldEffect.transform.Find("SnowParticles").GetComponent<ParticleSystem>().emissionRate == 50)
                {
                    coldEffect.transform.Find("SnowParticles").GetComponent<ParticleSystem>().emissionRate = 0;
                }

                if (coldEffectColor.a > 0)
                {
                    coldEffect.GetComponent<Image>().color = new Color(coldEffectColor.r, coldEffectColor.g, coldEffectColor.b, coldEffectColor.a -= .0025f);
                }
                else
                {
                    if (coldEffectColor.a < 0) coldEffect.GetComponent<Image>().color = new Color(coldEffectColor.r, coldEffectColor.g, coldEffectColor.b, 0);
                }
            }
        }
    }
}
