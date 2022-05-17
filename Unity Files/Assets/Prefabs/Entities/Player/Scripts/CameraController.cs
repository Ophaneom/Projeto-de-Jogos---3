using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject following;
    public float smoothness;

    private void FixedUpdate()
    {
        Vector3 _pos = this.transform.position;
        _pos.x += (following.transform.position.x - _pos.x) / smoothness;
        _pos.y += (following.transform.position.y - _pos.y) / smoothness;
        this.transform.position = _pos;
    }
}
