using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static List<string> AvailableLevels { get; private set; }

    private static List<string> AllLevels;
    private static Dictionary<string, LevelInfo> LevelInfos_;

    static LevelManager()
    {
        AllLevels = new List<string>();
        AvailableLevels = new List<string>();
        LevelInfos_ = new Dictionary<string, LevelInfo>();
        Reload();
    }

    public static void Reload()
    {
        AllLevels.Clear();
        AvailableLevels.Clear();
        LevelInfos_.Clear();

        bool addNextLevel = true;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var fullFileName = Path.GetFileName(SceneUtility.GetScenePathByBuildIndex(i));
            var fileName = Path.GetFileNameWithoutExtension(fullFileName);
            if (!fileName.StartsWith("Level"))
            {
                continue;
            }

            AllLevels.Add(fileName);

            var jsonPath = Path.Combine(Application.persistentDataPath, fileName + ".json");
            if (!File.Exists(jsonPath))
            {
                using (var file = File.CreateText(jsonPath))
                {
                    file.Write("{}");
                }
            }

            LevelInfo levelInfo = new LevelInfo(jsonPath);

            // Will at least always add the first level
            if (addNextLevel)
            {
                var levelName = fileName;
                if (levelInfo.ClearedLevel())
                {
                    levelName += string.Format(" | Best {0}", levelInfo.GetBestTime());
                }
                AvailableLevels.Add(levelName);
            }

            addNextLevel = levelInfo.ClearedLevel();

            LevelInfos_[fileName] = levelInfo;
        }

        Debug.Assert(AllLevels.Count > 0);

        AllLevels.Sort();
        if (AvailableLevels.Count == 0)
        {
            AvailableLevels.Add(AllLevels[0]);
        }
    }

    public static LevelInfo GetLevelInfo(string levelName)
    {
        Debug.Assert(LevelInfos_.ContainsKey(levelName));
        return LevelInfos_[levelName];
    }

    public static string GetNextLevel(string currentLevel)
    {
        int index = AllLevels.FindIndex((string level) =>
        {
            return level == currentLevel;
        });
        Debug.Assert(index != -1);
        index = Mathf.Clamp(index + 1, 0, AllLevels.Count - 1);
        return AllLevels[index];
    }

    public static string GetLevelByIndex(int index)
    {
        Debug.Assert(index >= 0 && index < AllLevels.Count);
        return AllLevels[index];
    }
}
