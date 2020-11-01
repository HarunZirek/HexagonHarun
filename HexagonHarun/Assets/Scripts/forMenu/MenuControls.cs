using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MenuControls : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private int count = 1;

    public GameObject image1;  //how to play page 1
    public GameObject image2;  //how to play page 2
    public GameObject image3;  //how to play page 3
    public GameObject howToPlayMenu; //how to play menu

    private void Update()
    {
        if(count==3)
        {
            buttonText.text = "DONE";
        }
        else
        {
            buttonText.text = "NEXT";
        }
    }

    //functions for main menu purposes
    public void OpenScene(int SceneIndex)
    {
        FindObjectOfType<audioManager>().Play("button");
        SceneManager.LoadScene(SceneIndex);
    }
    public void QuitGame()
    {
        FindObjectOfType<audioManager>().Play("button");
        Application.Quit();
    }

    public void openHowToPlay()
    {
        FindObjectOfType<audioManager>().Play("button");
        howToPlayMenu.SetActive(true);
        image1.SetActive(true);
    }

    //instead of adding different button i changed the function and name of the button with controlling it with count
    public void nextDone()
    {
        FindObjectOfType<audioManager>().Play("button");
        if (count==1)
        {
            image1.SetActive(false);
            image2.SetActive(true);
            count++;
        }
        else if(count==2)
        {
            image2.SetActive(false);
            image3.SetActive(true);
            count++;
        }
        else if(count==3)
        {
            image3.SetActive(false);
            howToPlayMenu.SetActive(false);
            count = 1;
        }
    }
}