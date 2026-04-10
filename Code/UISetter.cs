using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISetter : MonoBehaviour
{
    public Text levelNumberText;
    public Text livesLeftText;
    public Text timeLeftText;
    public Text scoreText;
    public Text highscoreText;
    private float timeLeft = 180;
    private bool gameEnded = false;
    public GameObject gameOverScreen;
    public GameObject singleplayerUI;
    public GameObject localMultiplayerUI;
    public GameObject onlineMultiplayerMenu;
    public GameObject onlineMultiplayerBar;
    public GameObject TimerPanel;
    public Text playerOneScore;
    public Text playerTwoScore;
    public Text gameOverText;
    private bool TimeRanOut = false;


    void Start()
    {
        if (!MainMenu.localMulti && !MainMenu.online){
            singleplayerUI.SetActive(true);
        livesLeftText.text = variables.lives.ToString();
        levelNumberText.text = "LEVEL " + variables.levelNumber;
        scoreText.text = variables.score.ToString();
        highscoreText.text = PlayerPrefs.GetFloat("highScore").ToString();
        }
        else if(MainMenu.localMulti){
            localMultiplayerUI.SetActive(true);
            playerOneScore.text = variables.playerOneScore.ToString();
            playerTwoScore.text = variables.playerTwoScore.ToString();
        }
        else if(MainMenu.online){
            onlineMultiplayerMenu.SetActive(true);
            onlineMultiplayerBar.SetActive(true);
            TimerPanel.SetActive(false);
}

    }

    private void Update()
    {
        if (!gameEnded && timeLeft > 0 && !MainMenu.online)
        {
            timeLeft -= Time.deltaTime;
            int minutes = (int)timeLeft / 60;
            int seconds = (int)timeLeft % 60;
            timeLeftText.text = minutes.ToString() + ":" + seconds.ToString("00");
        }
        else if (!gameEnded && timeLeft <= 0 && !TimeRanOut)
        {
            if(!MainMenu.localMulti){
                GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().TimeIsUp();
            }
            else{
                SceneManager.LoadScene("Game");
            }
            TimeRanOut = true;
        }
    }

    public void setScore(int scoreToAdd, int playerNumber)
    {
        if(!MainMenu.localMulti && !MainMenu.online){
                    variables.score += scoreToAdd;
        scoreText.text = variables.score.ToString();
        if (variables.score > variables.highscore)
        {
            variables.highscore = variables.score;
            highscoreText.text = variables.highscore.ToString();
            PlayerPrefs.SetFloat("highScore", variables.highscore);
            }
        }else if(MainMenu.localMulti){
            if(playerNumber==1){
                variables.playerOneScore += scoreToAdd;
                playerOneScore.text = variables.playerOneScore.ToString();
            }
            else{
                variables.playerTwoScore += scoreToAdd;
                playerTwoScore.text = variables.playerTwoScore.ToString();
            }
        }

    }

    public void PlayAgain()
    {
        variables.lives = 2;
        variables.levelNumber = 1;
        variables.score = 0;
        variables.playerOneScore = 0;
        variables.playerTwoScore = 0;
        variables.playerBombPower = 1;
        variables.playerMaxBombCount = 1;
        SceneManager.LoadScene("Game");
    }

    public void ExitToMainMenu()
    {
        variables.playerOneScore = 0;
        variables.playerTwoScore = 0;
        SceneManager.LoadScene("MainMenu");
        variables.playerBombPower = 1;
        variables.playerMaxBombCount = 1;
        PlayerPrefs.Save();
    }

    public void GameOver() {
        gameEnded = true;
        gameOverScreen.SetActive(true);
        if(MainMenu.localMulti){
            if(playerOneScore.text == variables.winMatches.ToString()){
                gameOverText.text = "Player 1 won";
            }
            else{
                gameOverText.text = "Player 2 won";
            }
        }
        else if(!MainMenu.localMulti && !MainMenu.online){
            gameOverText.text = "GAME OVER!";
        }
     }

}
