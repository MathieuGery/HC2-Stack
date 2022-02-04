using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static event Action OnItemCreated = delegate { };
    private ItemCreator[] itemCreators;
    private int creatorIndex;
    private ItemCreator currentCreator;
    public static int Level { get; set; }
    public GameOver GameOverScreen;
    private int HighScore;

    private void Awake()
    {
        itemCreators = FindObjectsOfType<ItemCreator>();
        Level = PlayerPrefs.GetInt("level", 1);
        HighScore = PlayerPrefs.GetInt("high_score", 0);
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Item.CurrentItem != null)
            {
                bool isGameOver = Item.CurrentItem.Stop();
                if (isGameOver)
                {
                    if (Score.score > HighScore)
                    {
                        PlayerPrefs.SetInt("high_score", Score.score);
                        PlayerPrefs.Save();
                        HighScore = Score.score;
                    }
                    YsoCorp.GameUtils.YCManager.instance.OnGameFinished(true);
                    GameOverScreen.Setup(Score.score, HighScore);
                } else {
                    creatorIndex = creatorIndex == 0 ? 1 : 0;
                    currentCreator = itemCreators[creatorIndex];
                    currentCreator.CreateItem(Level);
                    OnItemCreated();
                }
            }
        }
    }
}
