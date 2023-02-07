using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class SeedsChange : MonoBehaviour
{
    public Sprite[] seedSprite;
    public Image seedImage;
    [SerializeField] public TextMeshProUGUI currentSeed;
    private string[] seeds = { "None", "Cucumber", "Tomato" ,"Carrot","Pumpkin","Cabbage"};

    public int seedIndex;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && seedIndex < seeds.Length - 1)
        {
            seedIndex += 1;

            Debug.Log("Current seed: " + seeds[seedIndex]);

            currentSeed.text = "Current Seed: " + seeds[seedIndex];
            seedImage.sprite = seedSprite[seedIndex];

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && seedIndex > 1)
        {
            seedIndex -= 1;

            Debug.Log("Current seed: " + seeds[seedIndex]);
            currentSeed.text = "Current Seed: " + seeds[seedIndex];
            seedImage.sprite = seedSprite[seedIndex];
        }
    }
}
