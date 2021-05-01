using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDScript : MonoBehaviour
{
    private Text mTextTimer;
    private GameObject mNextLevelBtn;

    void Awake()
    {
        GameObject timerGameObject = GameObject.Find("Timer");
        mTextTimer = timerGameObject.GetComponent<Text>();

        mNextLevelBtn = GameObject.Find("Next");
        mNextLevelBtn.SetActive(false);
    }

    void Update()
    {
        TimeSpan duration = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        mTextTimer.text = "Timer: " + duration.ToString(@"m\:ss");
    }

    public void OnResetButtonClicked()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OnMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnNextLevelButtonClicked()
    {
        Debug.Log("OnNextLevelButtonClicked");
    }

    public void OnLevelCompleted()
    {
        Debug.Assert(mNextLevelBtn != null);
        mNextLevelBtn.SetActive(true);
    }
}
