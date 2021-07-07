using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarLoseScrene : MonoBehaviour
{
    public void PlayAgianClicked()
    {
        SceneManager.LoadScene("3DCar", LoadSceneMode.Single);
    }
}
