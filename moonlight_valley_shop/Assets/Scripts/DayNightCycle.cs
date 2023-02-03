using System.Collections;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float cycleDuration;
    private float dayNightBlend;
    private float elapsedTime;
    public int cycleCounter; 

    private Light sun;
    public Gradient lightColorGradient;
    public Gradient redColorGradient;

    void Start()
    {
        sun = GetComponent<Light>();
        elapsedTime = 0f;
        cycleCounter = 0;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= cycleDuration)
        {
            elapsedTime = 0f;
            cycleCounter++;
        }

        dayNightBlend = Mathf.Sin(2 * Mathf.PI * elapsedTime / cycleDuration) * 0.5f + 0.5f;
        sun.intensity = 1f - Mathf.Abs(dayNightBlend);

        if (sun.intensity > 0.5f)
        {
            sun.shadows = LightShadows.Hard;
        }
        else
        {
            sun.shadows = LightShadows.Soft;
        }

        if (cycleCounter !=0 && cycleCounter%2 == 0)
        {
            sun.color = redColorGradient.Evaluate(elapsedTime / cycleDuration);
            

        }
        else
        {
            sun.color = lightColorGradient.Evaluate(1f - (Mathf.Abs(dayNightBlend) * 2f));
        }
    }
}
