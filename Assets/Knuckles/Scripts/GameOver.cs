using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Text pointsText;
    public Text highScoreText;

    public void Setup(int score, int HighScore)
    {
        gameObject.SetActive(true);
        pointsText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + HighScore.ToString();
    }

    public void RestartButton()
    {
        PlayerPrefs.SetInt("level", Game.Level == 1 ? 2 : 1);
        PlayerPrefs.Save();
        Score.score = 0;
        Game.Level = Game.Level == 1 ? 2 : 1;
        SceneManager.LoadScene(0);
    }
}
