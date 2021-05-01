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
        int index = mSelectLevel.options.IndexOf(mSelectLevel.options[mSelectLevel.value]);
        string levelName = LevelManager.GetLevelByIndex(index);
        SceneManager.LoadScene(levelName);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
