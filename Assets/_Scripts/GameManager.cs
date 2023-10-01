using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public PlayerControls controls;
    public float startTime = 60f;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public GameObject endScreen;
    public TextMeshProUGUI finalScoreText;
   
    public int highestScore { get; private set; }

    private float timeRemaining;
    private int score;
    private bool isGameOver;
    [SerializeField] PostProcessChanger postProcessChanger;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            controls = new PlayerControls();
        }
    }
    private void Start()
    {
        highestScore = PlayerPrefs.GetInt("Highscore", 0);
        postProcessChanger.ResetPostProcessData();
        Cursor.visible = true;
        timeRemaining = startTime;
        isGameOver = false;
        StartCoroutine(Countdown());
        SoundManager.Instance.UnmuteAll();
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            if (!isGameOver)
            {
                timeRemaining -= 1;
                timerText.text = "Time: " + Mathf.Round(timeRemaining);
            }
        }
        GameOver();
    }

    public void AddScore(int points)
    {
        if (!isGameOver)
        {
            SoundManager.Instance.PlayScoreSound();
            score += points;
        }
        updateScoreText();
    }

    public void SubtractScore(int points)
    {
        if (!isGameOver)
        {
            SoundManager.Instance.PlayWrongSound();
            score -= points;
            if (score < 0)
            {
                score = 0;
            }
        }
        updateScoreText();
    }

    private void updateScoreText()
    {
        scoreText.text = "Score: " + score;
    }
    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("Highscore", 0);
        highestScore = 0;
    }
    private void GameOver()
    {
        isGameOver = true;
        controls.Gameplay.Disable();
        endScreen.SetActive(true);
        Cursor.visible = true;
        finalScoreText.text = "Final score " + score + " points!";
        postProcessChanger.ChangePostProcessData();
        SoundManager.Instance.MuteAll();
        if (score > highestScore)
        {
            PlayerPrefs.SetInt("Highscore", score);
            highestScore = score;
        }
        if (HighscoreManager.Instance.loggedIn)
        {
            HighscoreManager.Instance.SubmitHighscore(score);
        }
        // Disable user input here, depending on your game's mechanics
    }

    // public void RestartGame()
    // {
    //     SceneLoader.Instance.LoadGameScene();
    // }
    public void MainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
