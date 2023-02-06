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
    public int cycleCounter;

    private Light sun;

    public Gradient lightColorGradient;
    public Gradient redColorGradient;

    void Start()
    {
        sun = GetComponent<Light>();

        elapsedTime = 0f;
        cycleCounter = 2;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= cycleDuration)
        {
            elapsedTime = 0f;
            cycleCounter--;
        }

        dayNightBlend = Mathf.Sin(2 * Mathf.PI * (elapsedTime / cycleDuration + 0.25f)) * 0.5f + 0.5f;
        float redGradientEval = Mathf.Sin(2 * Mathf.PI * (elapsedTime / cycleDuration + 0.25f)) * 0.5f + 0.5f;
        sun.intensity = 1f - Mathf.Abs(dayNightBlend);

        if (sun.intensity > 0.5f)
        {
            sun.shadows = LightShadows.Hard;
        }
        else
        {
            sun.shadows = LightShadows.Soft;
            bloodMoon.text = cycleCounter.ToString();
        }

        if (cycleCounter == 0)
        {
            bloodMoon.text = "!";
            sun.color = redColorGradient.Evaluate(Mathf.Clamp(1f - (Mathf.Abs(dayNightBlend) * 2f), 0f, 1f));
        }
     

        if (cycleCounter < 0)
        {
            cycleCounter = 2;
        }
    }
}

