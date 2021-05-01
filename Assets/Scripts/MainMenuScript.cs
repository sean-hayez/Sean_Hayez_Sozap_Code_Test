using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private Dropdown mSelectLevel;

    void Awake()
    {
        var selectLevelGameObject = GameObject.Find("SelectLevel");
        mSelectLevel = selectLevelGameObject.GetComponent<Dropdown>();
        mSelectLevel.ClearOptions();

        LevelManager.Reload();
        mSelectLevel.AddOptions(LevelManager.AvailableLevels);
    }

    public void OnPlayButtonClicked()
    {
        var levelName = mSelectLevel.options[mSelectLevel.value].text;
        SceneManager.LoadScene(levelName);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
