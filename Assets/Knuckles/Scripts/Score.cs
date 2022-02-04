using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static int score;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
        text.text = "Click to Play" + "\nLevel: " + Game.Level.ToString();
        Game.OnItemCreated += IncScore;
    }

    private void OnDestroy()
    {
        Game.OnItemCreated -= IncScore;
    }

    private void IncScore()
    {
        score++;
        text.text = "Score: " + score + "\nLevel: " + Game.Level.ToString();
        if (score > 20)
        {
            float offset = 0.09f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + offset, Camera.main.transform.position.z);
        }
    }

}
