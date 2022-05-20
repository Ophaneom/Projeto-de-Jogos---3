using UnityEngine;

public class HitController : MonoBehaviour
{
    public int durability;
    public DropChart[] drops;
    private BoxCollider2D physicCollider, touchCollider;
    private Animator anim;
    private GameObject globalController;

    public Sprite test;

    private SpriteRenderer rend;
    private GameObject player;

    private void Start()
    {
        physicCollider = this.transform.Find("Physic_Collider").GetComponent<BoxCollider2D>();
        touchCollider = this.transform.Find("Touch_Collider").GetComponent<BoxCollider2D>();
        anim = this.transform.Find("Skin").GetComponent<Animator>();
        globalController = GameObject.Find("Global Controller");

        rend = this.transform.Find("Skin").GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (player.transform.position.x <= this.transform.position.x + rend.sprite.bounds.extents.x &&
                player.transform.position.x >= this.transform.position.x - rend.sprite.bounds.extents.x &&
                player.transform.position.y >= this.transform.position.y &&
                player.transform.position.y <= this.transform.position.y + rend.sprite.bounds.size.y)
        {
            if (rend.color.a > .35f) rend.color = new Color(1, 1, 1, rend.color.a - .01f);
        }
        else
        {
            if (rend.color.a < 1) rend.color = new Color(1, 1, 1, rend.color.a + .01f);
        }
    }

    public void Hit(int _toolStrength)
    {
        durability -= _toolStrength;
        if (durability <= 0) Down();
        else anim.Play("HitAnim");
    }

    public void Down()
    {
        physicCollider.enabled = false;
        touchCollider.enabled = false;
        DropItems();
        anim.Play("DownAnim");
    }

    public void DropItems()
    {
        foreach(DropChart _drop in drops)
        {
            globalController.GetComponent<InventoryController>().AddItemAuto(_drop.id, _drop.quantity);
        }
    }
}

[System.Serializable]
public class DropChart 
{
    public int id;
    public int quantity;
}
