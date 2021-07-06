using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenButton : MonoBehaviour
{
    
    public void PlayAgianClicked()
    {
        SceneManager.LoadScene("2DPlatformer", LoadSceneMode.Single);
    }
}
