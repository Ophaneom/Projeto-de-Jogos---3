using UnityEngine;

public class HitController : MonoBehaviour
{
    public int durability;
    public DropChart[] drops;
    private BoxCollider2D physicCollider, touchCollider;
    private Animator anim;
    private GameObject globalController;

    private void Start()
    {
        physicCollider = this.transform.Find("Physic_Collider").GetComponent<BoxCollider2D>();
        touchCollider = this.transform.Find("Touch_Collider").GetComponent<BoxCollider2D>();
        anim = this.transform.Find("Skin").GetComponent<Animator>();
        globalController = GameObject.Find("Global Controller");
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
