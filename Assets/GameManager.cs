using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    public TextMesh scoreText;
    public TextMesh multiplierText;
    public Background background;

    int score;
    int multiplier;
    int streak = 0;

	// Use this for initialization
	void Start () {
        UpdateScore(0);
        UpdateMultiplier(1);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void UpdateScore(int newScore)
    {
        scoreText.text = System.String.Format("{0}", newScore);
        score = newScore;
    }

    void UpdateMultiplier(int newMultiplier)
    {
        multiplierText.text = System.String.Format("{0} X", newMultiplier);
        multiplier = newMultiplier;
        // background.SetScoreMultiplyer(multiplier);
    }

    public void Hit()
    {
        streak += 1;
        if (streak > 25)
        {
            if (multiplier < 8)
            {
                UpdateMultiplier(multiplier * 2);
            }
            streak = 0;
        }
        UpdateScore(score + multiplier);
    }

    public void Miss()
    {
        streak = 0;
        int newMultiplier = multiplier > 1 ? multiplier / 2 : multiplier;
        UpdateMultiplier(newMultiplier);
    }
}
