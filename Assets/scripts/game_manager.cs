using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_manager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static game_manager Instance;

    public GameObject StartPage;
    public GameObject GameOverPage;
    public GameObject CountDownPage;
    public Text Score; 

    enum PageState
    {
        None,
        Start,
        GameOver,
        CountDown
    }
    int score = 0;
    bool gameOver = true;
    public bool GameOver { get { return gameOver; } }
    public int Scoree { get { return score; } }

    void Awake()
    {
        Instance = this;   
    }

    void OnEnable()
    {
        CountDownText.OnCountdownFinished += OnCountdownFinished;
        tap_controller.OnPlayerDied += OnPlayerDied;
        tap_controller.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable()
    {
        CountDownText.OnCountdownFinished -= OnCountdownFinished;
        tap_controller.OnPlayerDied -= OnPlayerDied;
        tap_controller.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        gameOver = false;
    }

    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore");
        if (score > savedScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        Score.text = score.ToString();
    }
    void SetPageState(PageState state)
    {
        switch (state)
        {
            case PageState.None:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.Start:
                StartPage.SetActive(true);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(false);
                break;
            case PageState.GameOver:
                StartPage.SetActive(false);
                GameOverPage.SetActive(true);
                CountDownPage.SetActive(false);
                break;
            case PageState.CountDown:
                StartPage.SetActive(false);
                GameOverPage.SetActive(false);
                CountDownPage.SetActive(true);
                break;

        }
    }

    public void ConfirmGameOver()
    {
        //activated when replay button is hit
        OnGameOverConfirmed();
        Score.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame()
    {
        //activated when play button is hit
        SetPageState(PageState.CountDown);
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
