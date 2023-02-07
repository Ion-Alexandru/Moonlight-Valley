using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour
{
    public float cycleDuration;
    private float dayNightBlend;

    [SerializeField] public TextMeshProUGUI bloodMoon;

    private float elapsedTime;

    private Light sun;

    public Gradient lightColorGradient;

    void Start()
    {
        sun = GetComponent<Light>();

        elapsedTime = 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= cycleDuration)
        {
            elapsedTime = 0f;
        }

        dayNightBlend = Mathf.Sin(2 * Mathf.PI * (elapsedTime / cycleDuration + 0.25f)) * 0.5f + 0.5f;
        sun.intensity = 1f - Mathf.Abs(dayNightBlend);

        if (sun.intensity > 0.5f)
        {
            sun.shadows = LightShadows.Hard;
        }
        else
        {
            sun.shadows = LightShadows.Soft;
        }

    }
}

