using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    private hexagonGrid grid;
    private bool isPaused = false;

    private void Start()
    {
        grid = FindObjectOfType<hexagonGrid>();
    }

    public void PauseMenuControl()
    {
        //if pauseMenu is open closes it if it is closed opens it

        FindObjectOfType<audioManager>().Play("button");
        if (isPaused==false)
        {
            pauseMenuUI.SetActive(true);
            grid.canPlay = false;
            isPaused = true;
        }
        else if(isPaused==true)
        {
            pauseMenuUI.SetActive(false);
            grid.canPlay = true;
            isPaused = false;
        }
    }

    //functions and scene controls when the pause menu is open
    public void newGame()
    {
        FindObjectOfType<audioManager>().Play("button");
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
    }
    public void mainMenu(int sceneNumber)
    {
        FindObjectOfType<audioManager>().Play("button");
        SceneManager.LoadScene(sceneNumber);
    }
}
