using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;

    void Update() 
    {
        //keeping the score in update so player can see it in UI immediatly
        scoreText.text = score.ToString();
    }

    //the function that adds score for each destroyed hexagon
    public void AddScore(int addAmount)
    {
        score += addAmount;
    }


}
