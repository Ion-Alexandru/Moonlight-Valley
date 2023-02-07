using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GardeningBehaviour gardeningBehaviour;
    public PlayerScript player;
    private float saveTimer = 10f;
    public FloatingTextManager floatingTextManager;
    public static GameManager instance;
    public RectTransform hitpointBar;
    public UnityEngine.UI.Text moonCoinsText;
    int moonCoins;

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        instance = this;
        LoadState();
    }

    // Start is called before the first frame update
    void Start() {
        //Do something with the value stored in `sharedValue`
        int moonCoins = StaticClass.moonCoins;
    }

    // Save State
    // INT preferedSkin
    // INT pesos
    public void SaveState()
    {
        for (int i = 0; i < gardeningBehaviour.vegetables.Length; i++)
        {
            PlayerPrefs.SetInt("ArrayValue_" + i, gardeningBehaviour.vegetables[i]);
        }
        Debug.Log("Game saved");
    }

    private void Update()
    {
        moonCoinsText.text = "Mooncoins: " + moonCoins.ToString();

        saveTimer -= Time.deltaTime;

        if (saveTimer <= 0)
        {
            SaveState();
            saveTimer = 10f;
        }
    }

    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    public void LoadState()
    {
        if (!PlayerPrefs.HasKey("Save State"))
            return;

        int[] arrayToLoad = new int[gardeningBehaviour.vegetables.Length];

        for (int i = 0; i < gardeningBehaviour.vegetables.Length; i++)
        {
            arrayToLoad[i] = PlayerPrefs.GetInt("ArrayValue_" + i);
        }
        PlayerPrefs.GetInt("Coins", moonCoins);
    }

    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoints / (float)player.maxHitpoints;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }
}
