using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        Debug.Log("Loading game scene...");
        SceneManager.LoadScene(1);
    }
}
