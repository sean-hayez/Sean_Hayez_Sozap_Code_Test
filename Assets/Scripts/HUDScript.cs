using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    private string mLevelName;
    private LevelInfo mLevelInfo;

    private Text mTextTimer;
    private GameObject mNextLevel;
    private GameObject mLevelCompleted;

    void Awake()
    {
        mLevelName = SceneManager.GetActiveScene().name;
        mLevelInfo = LevelManager.GetLevelInfo(mLevelName);
        mLevelInfo.IncrementAttempts();

        var timerGameObject = GameObject.Find("Timer");
        mTextTimer = timerGameObject.GetComponent<Text>();

        var attemptsGameObject = GameObject.Find("Attempts");
        var attemptsText = attemptsGameObject.GetComponent<Text>();
        attemptsText.text = "Attempts: " + mLevelInfo.GetAttempts();

        mLevelCompleted = GameObject.Find("LevelCompleted");
        mLevelCompleted.SetActive(false);

        mNextLevel = GameObject.Find("Next");
        if (!mLevelInfo.ClearedLevel())
        {
            mNextLevel.SetActive(false);
        }
    }

    void Update()
    {
        if (!mLevelCompleted.activeSelf)
        {
            TimeSpan duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            mTextTimer.text = "Timer: " + duration.ToString(@"m\:s");
        }
    }

    public void OnResetButtonClicked()
    {
        SceneManager.LoadScene(mLevelName);
    }

    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene(LevelManager.GetNextLevel(mLevelName));
    }

    public void LevelCompleted()
    {
        if (!mLevelCompleted.activeSelf)
        {
            Debug.Assert(mNextLevel != null);
            mNextLevel.SetActive(true);
            mLevelInfo.SetTime((int)Time.timeSinceLevelLoad);
            mLevelCompleted.SetActive(true);
        }
    }
}
