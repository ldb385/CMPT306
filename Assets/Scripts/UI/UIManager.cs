using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    GameObject[] pauseObjects;

    // pathnames to scenes that will be loaded using UI
    private string scenePath = "Assets/Scenes/MapPcgDemo.unity";
    private string mainMenuPath = "Assets/Scene/MainMenu.unity";

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        hidePaused();
    }

    // Update is called once per frame
    void Update()
    {
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

    // load main menu
    public void LoadMainMenu() {
        {
            SceneManager.LoadScene(mainMenuPath);
        }
    }

}
