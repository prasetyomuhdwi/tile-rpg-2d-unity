using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;

    private GameObject ui;
    private GameObject gameManager;
    private GameObject player;
    private GameObject cam;

    private void Start()
    {
        player = GameObject.Find("Player");
        ui = GameObject.Find("UI");
        gameManager = GameObject.Find("GameManager");
        cam = GameObject.Find("Main Camera");
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        
        Destroy(ui);
        Destroy(gameManager);
        Destroy(cam);
        Destroy(player);

        SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
        Debug.Log("Quitting game ...");
        Application.Quit();
    }


}
