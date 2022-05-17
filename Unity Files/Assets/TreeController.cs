using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeController : MonoBehaviour
{
    public int durability;
    private BoxCollider2D physicCollider, touchCollider;
    private Animator anim;
    private SpriteRenderer rend;
    private GameObject player;
    private GameObject globalController;

    private void Start()
    {
        physicCollider = this.transform.Find("Physic_Collider").GetComponent<BoxCollider2D>();
        touchCollider = this.transform.Find("Touch_Collider").GetComponent<BoxCollider2D>();
        anim = this.transform.Find("Skin").GetComponent<Animator>();
        rend = this.transform.Find("Skin").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        globalController = GameObject.Find("Global Controller");
    }

    public void Chop(int _toolStrength)
    {
        durability -= _toolStrength;
        if (durability <= 0) Chopped();
        else anim.Play("TreeChoppingAnim");
    }

    public void Chopped()
    {
        physicCollider.enabled = false;
        touchCollider.enabled = false;
        DropItems();
        anim.Play("TreeChoppedAnim");
    }

    public void DropItems()
    {
        globalController.GetComponent<InventoryController>().AddItemAuto(7, 3);
    }
}
