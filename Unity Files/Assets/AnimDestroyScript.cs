using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimDestroyScript : MonoBehaviour
{
    public void DestroyThis()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
