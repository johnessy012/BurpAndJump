using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour {

    private Text _bestScore;
    private Text _currentScore;

    private void Start()
    {
        _bestScore = GameObject.Find("BestScore").GetComponent<Text>();
        _currentScore = GameObject.Find("CurrentScore").GetComponent<Text>();
        UpdateBestScore();
    }

    public void UpdateBestScore()
    {
        _bestScore.text = "Best Score" + PlayerPrefs.GetFloat("BestScore").ToString();
    }

    public void UpdateCurrentScore(float score)
    {
        _currentScore.text = "Current Score " + score.ToString("000");
    }
}
