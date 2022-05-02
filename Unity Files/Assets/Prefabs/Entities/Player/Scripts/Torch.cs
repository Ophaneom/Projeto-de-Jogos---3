using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public GameObject cam;

    private void Update()
    {
        this.transform.position = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }
}
