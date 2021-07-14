using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThreeLevelLose : MonoBehaviour
{

    public void PlayAgainPressed()
    {
        SceneManager.LoadScene("3DShooter", LoadSceneMode.Single);
    }

}
