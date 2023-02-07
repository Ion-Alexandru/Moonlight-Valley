using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(LoadScene);
    }
    public void LoadScene()
    {
        SceneManager.LoadScene("MainGame");
    }
}
