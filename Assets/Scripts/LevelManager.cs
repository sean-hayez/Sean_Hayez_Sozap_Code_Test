using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static List<string> AvailableLevels { get; private set; }

    private static List<string> AllLevels_;
    private static Dictionary<string, LevelInfo> LevelInfos_;

    static LevelManager()
    {
        AvailableLevels = new List<string>();
        AllLevels_ = new List<string>();
        LevelInfos_ = new Dictionary<string, LevelInfo>();
        Reload();
    }

    public static void Reload()
    {
        AvailableLevels.Clear();
        AllLevels_.Clear();
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

            AllLevels_.Add(fileName);

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

        Debug.Assert(AllLevels_.Count > 0);

        AllLevels_.Sort();
        if (AvailableLevels.Count == 0)
        {
            AvailableLevels.Add(AllLevels_[0]);
        }
    }

    public static LevelInfo GetLevelInfo(string levelName)
    {
        Debug.Assert(LevelInfos_.ContainsKey(levelName));
        return LevelInfos_[levelName];
    }

    public static string GetNextLevel(string currentLevel)
    {
        int index = AllLevels_.FindIndex((string level) =>
        {
            return level == currentLevel;
        });
        Debug.Assert(index != -1);
        index = Mathf.Clamp(index + 1, 0, AllLevels_.Count - 1);
        return AllLevels_[index];
    }

    public static string GetLevelByIndex(int index)
    {
        Debug.Assert(index >= 0 && index < AllLevels_.Count);
        return AllLevels_[index];
    }
}
