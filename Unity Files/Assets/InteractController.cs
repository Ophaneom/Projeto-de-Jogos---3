using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private float maxDistanceToInteract;
    private GameObject globalController;

    private void Start()
    {
        globalController = GameObject.Find("Global Controller");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                if (globalController.GetComponent<InventoryController>().actualItem != null)
                {
                    if (hit.collider.tag == "Tree" && 
                        globalController.GetComponent<InventoryController>().actualItem.toolSettings.action == 2 && 
                        Vector2.Distance(this.transform.position, hit.collider.gameObject.transform.position) <= maxDistanceToInteract)
                    {
                        hit.collider.transform.parent.gameObject.GetComponent<TreeController>().Chop(globalController.GetComponent<InventoryController>().actualItem.toolSettings.strength);
                    }
                }
            }
        }
    }
}
