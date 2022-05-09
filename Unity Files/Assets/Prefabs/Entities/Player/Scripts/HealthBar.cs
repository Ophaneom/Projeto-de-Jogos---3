using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int maxLife;
    public int currenteLife;

    void Update()
    {
        transform.localScale = new Vector3(currenteLife * 100 / maxLife, 15, 1);
    }
}
