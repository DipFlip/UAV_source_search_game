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
        postProcessChanger.ResetPostProcessData();
        Cursor.visible = false;
        timeRemaining = startTime;
        isGameOver = false;
        StartCoroutine(Countdown());
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
    private void GameOver()
    {
        isGameOver = true;
        controls.Gameplay.Disable();
        endScreen.SetActive(true);
        Cursor.visible = true;
        finalScoreText.text = "You found " + score + " sources!";
        postProcessChanger.ChangePostProcessData();
        // Disable user input here, depending on your game's mechanics
    }

    public void RestartGame()
    {
        SceneLoader.Instance.LoadGameScene();
    }
    public void MainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
