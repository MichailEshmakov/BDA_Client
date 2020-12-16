using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    [SerializeField] int scorePerFaller;
    [SerializeField] int startLives;

    [SerializeField] Text endScoreText;

    [SerializeField] GameObject startUI;
    [SerializeField] GameObject playUI;
    [SerializeField] GameObject gameoverUI;

    private int score;
    private int Score 
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = $"Score: {score}";
        }
    }

    private int lives;
    private int Lives
    {
        get
        {
            return lives;
        }
        set
        {
            lives = value;
            livesText.text = $"Lives: {lives}";
        }
    }

    public enum gameStatus
    {
        start, play, gameover
    }

    public gameStatus CurrentState { get; private set; }

    public static event Action OnStartPlay;
    public static event Action OnGameover;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(400, 640, false);
        CurrentState = gameStatus.start;
    }

    public void PlayGame()
    {
        playUI.SetActive(true);
        startUI.SetActive(false);
        gameoverUI.SetActive(false);
        Lives = startLives;
        Score = 0;
        CurrentState = gameStatus.play;
        OnStartPlay?.Invoke();
    }

    private void Gameover()
    {
        CurrentState = gameStatus.gameover;
        playUI.SetActive(false);
        gameoverUI.SetActive(true);
        endScoreText.text = $"Your score: {Score}";
        OnGameover?.Invoke();
    }

    internal void AddScore()
    {
        Score += scorePerFaller;
    }

    internal void MinusLife()
    {
        Lives -= 1;
        if (Lives <= 0)
        {
            Gameover();
        }
    }


}
