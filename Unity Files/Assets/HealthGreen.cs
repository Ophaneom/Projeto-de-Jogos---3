using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthGreen : MonoBehaviour
{
    public int health;
    public Transform healthBar;
    public GameObject healthBarObject;

    private Vector3 healthBarScale;
    private float healthPercent;

    void Start()
    {
        healthBarScale = healthBar.localScale;
        healthPercent = healthBarScale.x / health;
    }

    void UpdateHealth()
    {
        healthBarScale.x = healthPercent * health;
        healthBar.localScale = healthBarScale;
    }

    void Flip()
    {
        healthBarObject.transform.localScale = new Vector3(healthBarObject.transform.localScale.x * -1,
                                                           healthBarObject.transform.localScale.y,
                                                           healthBarObject.transform.localScale.z);
    }
}
