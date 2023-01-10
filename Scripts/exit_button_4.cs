using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exit_button : MonoBehaviour
{
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

}
