using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{    
    public GameManager gameManager;
    public Button shopButton;

    public object ShopScene { get; private set; }

    void Start()
    {
        shopButton.onClick.AddListener(LoadScene);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("ShopScene");
        PlayerPrefs.SetInt("MoonCoins", StaticClass.moonCoins);
    }
}
