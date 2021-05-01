using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    private string mLevelName;
    private LevelInfo mLevelInfo;

    private Text mTextTimer;
    private GameObject mNextLevelBtn;

    void Awake()
    {
        mLevelName = SceneManager.GetActiveScene().name;
        mLevelInfo = LevelManager.GetLevelInfo(mLevelName);

        GameObject timerGameObject = GameObject.Find("Timer");
        mTextTimer = timerGameObject.GetComponent<Text>();

        mNextLevelBtn = GameObject.Find("Next");
        if (!mLevelInfo.ClearedLevel())
        {
            mNextLevelBtn.SetActive(false);
        }
    }

    void Update()
    {
        TimeSpan duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        mTextTimer.text = "Timer: " + duration.ToString(@"m\:s");
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

    public void OnLevelCompleted()
    {
        Debug.Assert(mNextLevelBtn != null);
        mNextLevelBtn.SetActive(true);
        mLevelInfo.SetTime((int)Time.timeSinceLevelLoad);
    }
}
