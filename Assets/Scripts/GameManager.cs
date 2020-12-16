using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    [SerializeField] int scorePerFaller;
    [SerializeField] int startLives;

    [SerializeField] GameObject startUI;
    [SerializeField] GameObject playUI;
    [SerializeField] GameObject gameoverUI;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Text endScoreText;
    [SerializeField] InputField nameField;
    [SerializeField] GameObject namePanel;

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

    public void AddScore()
    {
        Score += scorePerFaller;
    }

    public void MinusLife()
    {
        Lives -= 1;
        if (Lives <= 0)
        {
            Gameover();
        }
    }

    public async void SubmitName()
    {
        await SaveScore();
        CloseNamePanel();
    }

    private async Task SaveScore()
    {
        loadingPanel.SetActive(true);
        await NetworkManager.Instance.SaveScore(nameField.text, Score);
        loadingPanel.SetActive(false);

        // TODO: Добавить сообщение об успешности/неуспешности сохранения
    }

    public void ShowNamePanel()
    {
        namePanel.SetActive(true);
    }

    public void CloseNamePanel()
    {
        namePanel.SetActive(false);
    }
}
