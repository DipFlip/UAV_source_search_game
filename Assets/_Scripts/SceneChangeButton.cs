using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void LoadGameScene()
    {
        SceneLoader.Instance.LoadGameScene();
    }
    public void LoadMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }
}
