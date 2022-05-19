using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionController : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnBecameVisible()
    {
        gameObject.SetActive(true);
    }
}
