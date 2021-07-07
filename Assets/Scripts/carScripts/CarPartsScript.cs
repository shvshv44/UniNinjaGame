using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarPartsScript : MonoBehaviour
{
    public int playerLives = 3;
    public Text lives;
    public GameObject mainCamera;
    public GameObject winningCamera;

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
            Debug.Log("camera");
            mainCamera.SetActive(false);
            winningCamera.SetActive(true);
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
        lives.text = "";
        for (int i = 0; i < playerLives; i++)
        {
            lives.text += "♥ ";
        }
    }
}
