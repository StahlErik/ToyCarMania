using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{

    private int playerScore;
    public float timeLeft = 30f;
    public float timeIncrease = 5f;
    public Text timeLeftText;
    public Text scoreText;
    private bool gameEnded = false;

    void Start()
    {
        scoreText = GameObject.Find("ScoreCounter").GetComponent<Text>();
        timeLeftText = GameObject.Find("TimeCounter").GetComponent<Text>();
        playerScore = 0;
        scoreText.text = "Score: " + playerScore;
        timeLeftText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timeLeftText.text = Mathf.CeilToInt(timeLeft).ToString();
        if (timeLeft < 0f && !gameEnded)
        {
            GameObject.Find("Game Handler").GetComponent<GameHandler>().gameOver(playerScore);
            gameEnded = true;
        }
    }

    public void IncreaseScoreAndTime()
    {
        playerScore = playerScore + 1;
        scoreText.text = "Score: " + playerScore;
        timeLeft += timeIncrease;
    }
}
