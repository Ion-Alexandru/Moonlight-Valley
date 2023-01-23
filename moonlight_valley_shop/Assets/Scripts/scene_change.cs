using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_change : MonoBehaviour
{
    public Shop_Script shopScript;

   public void nextScene()
    {
        
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
