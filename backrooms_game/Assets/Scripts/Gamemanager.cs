using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public int totalCollections = 1;
    int currentCollections = 0;
    public GameObject gameobject;
    public Canvas loseScreen;
    public Canvas winScreen;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "collect")
        {
            currentCollections++;
            gameobject = GameObject.Find(other.gameObject.name);
            gameobject.SetActive(false);
            Debug.Log(currentCollections);
        }
        if (other.tag == "Finish" && currentCollections == totalCollections)
        {
            winScreen.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "enemy")
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            loseScreen.enabled = true;
            Time.timeScale = 0.0f;
        }
    }


    public void quit()
    {
        Application.Quit();
    }
    public void retry()
    {
        currentCollections = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
