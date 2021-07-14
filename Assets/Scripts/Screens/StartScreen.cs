using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    
    public void PlayPressed()
    {
        SceneManager.LoadScene("3DCar", LoadSceneMode.Single);
    }

    public void QuitPressed()
    {
        Application.Quit();
    }

}
