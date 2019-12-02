using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class UIManager : MonoBehaviour
{
    GameObject[] pauseObjects;
    GameObject[] gameOverObjects;
    GameObject[] winObjects;
    GameObject player;
    GameObject boss;

    bool isAlive;
    bool isDead;


    // pathnames to scenes that will be loaded using UI
    private string scenePath = "Assets/Scenes/LevelOne.unity";
    private string mainMenuPath = "Assets/Scenes/MainMenu.unity";

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        gameOverObjects = GameObject.FindGameObjectsWithTag("ShowOnGameOver");
        winObjects = GameObject.FindGameObjectsWithTag("ShowOnWin");
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");

        hidePaused();
        hideGameOver();
        hideWin();
    }

    // Update is called once per frame
    void Update()
    {
        isAlive = player.GetComponent<Player>().isAlive;
        
        if(boss != null)
        {
            isDead = boss.GetComponent<BossData>().isDead;
        }

        // use p button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                showPaused();
            } else if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hidePaused();
            }
        }

        // if player dies, display Game Over screen
        if (isAlive == false)
        {
            Time.timeScale = 0;
            showGameOver();
        }

        // if player kills boss, display win screen
        if (isDead == true)
        {
            Time.timeScale = 0;
            showWin();
        }
    }

    // restart game/scene
    public void Restart()
    {
        SceneManager.LoadScene(scenePath);
    }


    // controls pausing of the scene
    public void pausedControl()
    {
        if(Time.timeScale == 1)
        {
            Time.timeScale = 0;
            showPaused();
        }else if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
            hidePaused();
        }
    }

    // show paused menu
    public void showPaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(true);
        }
    }

    // hide paused manu
    public void hidePaused()
    {
        foreach(GameObject g in pauseObjects)
        {
            g.SetActive(false);
        }
    }

    public void showGameOver()
    {
        
        foreach(GameObject g in gameOverObjects)
        {
            g.SetActive(true);
        }
    }

    public void hideGameOver()
    {
        foreach(GameObject g in gameOverObjects)
        {
            g.SetActive(false);
        }
    }

    // load main menu
    public void LoadMainMenu() {
        {
            SceneManager.LoadScene(mainMenuPath);
        }
    }

    // show win screen
    public void showWin()
    {
        foreach(GameObject g in winObjects)
        {
            g.SetActive(true);
        }
    }

    // hide win screen
    public void hideWin()
    {
        foreach (GameObject g in winObjects)
        {
            g.SetActive(false);
        }
    }

}
