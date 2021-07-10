using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarPartsScript : MonoBehaviour
{
    public int playerLives = 3;
    public Text lives;
    public GameObject mainCamera;
    public GameObject winningCamera;
    public GameObject directionLight;

    void Start()
    {
        winningCamera.SetActive(false);
        lives.text = "";
        for (int i = 0; i < playerLives; i++)
        {
            lives.text += "♥ ";
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "hurdle")
        {
            if (playerLives >= 1)
            {
                playerLives--;
                changeLives();
            }
        }

        if (col.gameObject.tag == "Finish")
        {
            mainCamera.SetActive(false);
            winningCamera.SetActive(true);
        }

        if (col.gameObject.tag == "water")
        {
            lose();
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Finish")
        {
            Debug.Log("camera");
            mainCamera.SetActive(false);
            winningCamera.SetActive(true);
        }
    }
    void changeLives()
    {

        if (playerLives == 0)
        {
            lose();
        }

        lives.text = "";
        for (int i = 0; i < playerLives; i++)
        {
            lives.text += "♥ ";
        }
    }

    private void lose() {
        directionLight.transform.rotation = Quaternion.Euler(50, -30, 0);
        SceneManager.LoadScene("3DCarLose", LoadSceneMode.Single);
    }
}
