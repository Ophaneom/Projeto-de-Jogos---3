using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float timeVelocity;
    public int actualDay;
    public float actualTick;
    [SerializeField] private GameObject globalLightObj;
    [Range(0, 1)] public float nightIntensity;
    public bool pauseClock;

    [Header("Sunset Settings")]
    public float sunsetTickStart;

    [Header("Night Settings")]
    public float nightTickStart;

    [Header("Sunrise Settings")]
    public float sunriseTickStart;
    public float sunriseTickEnd;

    private float[] intervals = new float[2];
    private Light2D globalLight;

    private void Awake()
    {
        intervals[0] = nightIntensity / (nightTickStart - sunsetTickStart);
        intervals[1] = nightIntensity / (sunriseTickEnd - sunriseTickStart);
    }

    private void Start()
    {
        globalLight = globalLightObj.GetComponent<Light2D>();
    }

    private void FixedUpdate()
    {
        if (pauseClock) return;
        if (actualTick <= sunriseTickEnd)
        {
            actualTick += timeVelocity;

            if (actualTick < sunsetTickStart)
            {
                if (globalLight.intensity != 1) globalLight.intensity = 1;
            }
            else if (actualTick >= sunsetTickStart && actualTick < nightTickStart)
            {
                globalLight.intensity -= intervals[0];
            }
            else if (actualTick >= nightTickStart && actualTick < sunriseTickStart)
            {
                if (globalLight.intensity != 1 - nightIntensity) globalLight.intensity = 1 - nightIntensity;
            }
            else if (actualTick >= sunriseTickStart && actualTick < sunriseTickEnd)
            {
                globalLight.intensity += intervals[1];
            }
        }
        else
        {
            actualTick = 0;
            actualDay++;
        }
    }
}
