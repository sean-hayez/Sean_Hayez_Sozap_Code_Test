using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private Dropdown mSelectLevel;

    void Start()
    {
        var selectLevelGameObject = GameObject.Find("SelectLevel");
        mSelectLevel = selectLevelGameObject.GetComponent<Dropdown>();

        List<string> levels = new List<string>();
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var fileName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (fileName.StartsWith("Level"))
            {
                levels.Add(fileName);
            }
        }

        mSelectLevel.ClearOptions();
        mSelectLevel.AddOptions(levels);
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
