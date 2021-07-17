using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WinScrene : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayAgianClicked()
    {
        SceneManager.LoadScene("3DCar", LoadSceneMode.Single);
    }

    // Update is called once per frame
    public void QuitPressed()
    {
        Application.Quit();
    }
}
