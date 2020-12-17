using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GameManager : Singleton<GameManager>
{
    // TODO: Вынести работу с UI в UiManager
    [SerializeField] Text scoreText;
    [SerializeField] Text livesText;
    [SerializeField] int scorePerFaller;
    [SerializeField] int startLives;

    [SerializeField] GameObject startUI;
    [SerializeField] GameObject playUI;
    [SerializeField] GameObject gameoverUI;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Text endScoreText;
    [SerializeField] Text rankText;
    [SerializeField] InputField nameField;
    [SerializeField] GameObject namePanel;
    [SerializeField] Leaderbord leaderbord;

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

    private List<ScoreLine> scoreLines;
    private int rank;

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

    private async void Gameover()
    {
        CurrentState = gameStatus.gameover;
        OnGameover?.Invoke();
        loadingPanel.SetActive(true);
        scoreLines = (await NetworkManager.Instance.LoadScoreLines()).ToList();
        loadingPanel.SetActive(false);
        playUI.SetActive(false);
        gameoverUI.SetActive(true);
        endScoreText.text = $"Your score: {Score}";
        rank = ComputeRank(scoreLines, Score);
        rankText.text = $"RANKED: {rank}";
    }

    private int ComputeRank(List<ScoreLine> scoreLines, int score)
    {
        int nearestRank = scoreLines.IndexOf(scoreLines.OrderBy(line => Mathf.Abs(line.score - score)).First());
        int rankShift = scoreLines[nearestRank].score > score ? 2 : 1;
        return nearestRank >= 0 ? (nearestRank + rankShift) : 1;
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
        string name = nameField.text;
        await NetworkManager.Instance.SaveScore(name, Score);
        scoreLines.Insert(rank - 1, new ScoreLine(name, Score));
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

    public void ShowLeaderbord()
    {
        leaderbord.gameObject.SetActive(true);
        leaderbord.Refresh(scoreLines);
    }

    public void CloseLeaderbord()
    {
        leaderbord.gameObject.SetActive(false);
    }
}
