using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {

        // PlayerPrefs.DeleteAll();
        instance = this;
        LoadState();
    }

    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> weaponSprites;
    public List<int> Prices;

    // References
    public Player player;
    public Weapon weapon;
    public FloatingTextManager floatingTextManager;
    public RectTransform hitpointBar;

    // Logic
    public int seeds;

    private float saveTimer = 10f;

    // Spawner
    public GameObject enemyPrefab;
    public float spawnRate = 1f;

    private float spawnTimer = 0f;
    private int spawnCount = 0;
    private const int MAX_SPAWNS = 5;


    // Hitpoint Bar
    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Save State
    // INT preferedSkin
    // INT pesos
    public void SaveState()
    {
        string s = " ";

        s += "0" + "|";
        s += seeds.ToString() + "|";
        s += "0";

        PlayerPrefs.SetString("Save State", s);
        Debug.Log("Game saved");
    }

    private void Update()
    {
        saveTimer -= Time.deltaTime;

        if (saveTimer <= 0)
        {
            SaveState();
            saveTimer = 10f;
        }

        /*
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnRate;
        }
        */
    }

    public void LoadState()
    {
        if (!PlayerPrefs.HasKey("Save State"))
            return;

        string[] data = PlayerPrefs.GetString("Save State").Split('|');

        // Change player skin
        seeds = int.Parse(data[1]);

        Debug.Log("SaveState");
    }

    /*
    private void SpawnEnemy()
    {

        // Check if the maximum number of spawns has been reached
        if (spawnCount >= MAX_SPAWNS)
        {
            return;
        }

        Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        Vector3 screenPos = Camera.main.WorldToScreenPoint(spawnPosition);

        if (screenPos.x >= 0 && screenPos.x <= Screen.width &&
            screenPos.y >= 0 && screenPos.y <= Screen.height)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    } */
}