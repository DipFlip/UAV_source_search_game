using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro; // Add this line to use TextMeshPro Input Field

public class HighscoreManager : MonoBehaviour
{
    public TMP_InputField usernameInputField; // Change to TMP_InputField
    public GameObject leaderboardPanel;
    public TextMeshProUGUI leaderboardEntryTemplate;
    public int maxResults = 10;
    public GameObject registerButton;
    public GameObject logoutButton;
    public bool loggedIn { get; private set; }

    public static HighscoreManager Instance;
    private string username;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        username = PlayerPrefs.GetString("Username", string.Empty);
        if (!string.IsNullOrEmpty(username))
        {
            // usernameInputField.text = storedUsername;
            Login();
        }
    }
    public void Register()
    {
        username = usernameInputField.text;
        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username cannot be empty.");
            return;
        }
        Login();
    }

    public void Login()
    {
        Debug.Log("Logging in as " + username + "...");
        var request = new LoginWithCustomIDRequest
        {
            CustomId = username,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log($"Logged in successfully!");
        if (result == null || result.InfoResultPayload == null || result.InfoResultPayload.PlayerProfile == null || result.InfoResultPayload.PlayerProfile.DisplayName == null)
        {
            // This account was just created and doesn't have a display name yet
            UpdateDisplayName();
        }
        else
        {
            Debug.Log($"Welcome, {result.InfoResultPayload.PlayerProfile.DisplayName}!");
            // GetLeaderboard(0, maxResults, DisplayLeaderboard);
        }
        PlayerPrefs.SetString("Username", username);
        usernameInputField.gameObject.SetActive(false);
        registerButton.SetActive(false);
        logoutButton.SetActive(true);
        loggedIn = true;
    }

    public void Logout()
    {
        Debug.Log("Logged out successfully!");
        usernameInputField.gameObject.SetActive(true);
        registerButton.SetActive(true);
        logoutButton.SetActive(false);
        loggedIn = false;
        GameManager.Instance.ResetHighscore();
    }

    private void UpdateDisplayName()
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = username
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnUpdateDisplayNameSuccess, OnError);
    }

    private void OnUpdateDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"Welcome, {result.DisplayName}!");
        SubmitHighscore(GameManager.Instance.highestScore);
    }

    public void SubmitHighscore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Highscore",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnHighscoreSubmitted, OnError);
    }

    private void OnHighscoreSubmitted(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Highscore submitted successfully!");
        StartCoroutine(WaitAndUpdateLeaderboard());
    }

    private IEnumerator WaitAndUpdateLeaderboard()
    {
        yield return new WaitForSeconds(1.0f); // Wait for 2 seconds before updating the leaderboard
        GetLeaderboard(0, maxResults, DisplayLeaderboard);
    }

    public void GetLeaderboard(int startPosition, int maxResults, System.Action<List<PlayerLeaderboardEntry>> onLeaderboardReceived)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "Highscore",
            StartPosition = startPosition,
            MaxResultsCount = maxResults
        };

        PlayFabClientAPI.GetLeaderboard(request, result => {
            onLeaderboardReceived?.Invoke(result.Leaderboard);
        }, OnError);
    }

    public void DisplayLeaderboard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        // Clear previous leaderboard entries
        foreach (Transform child in leaderboardPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Display new leaderboard entries
        foreach (var entry in leaderboardEntries)
        {
            Debug.Log(entry);
            TextMeshProUGUI entryText = Instantiate(leaderboardEntryTemplate, leaderboardPanel.transform);
            entryText.text = $"{entry.Position + 1}. {entry.DisplayName} - {entry.StatValue}";
            // entryText.gameObject.SetActive(true);

        }
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError($"PlayFab Error: {error.GenerateErrorReport()}");
    }
}
