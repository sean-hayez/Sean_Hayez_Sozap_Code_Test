using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static List<string> AllLevels { get; private set; }
    public static List<string> AvailableLevels { get; private set; }

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

        bool levelCleared = true;
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

            if (levelCleared)
            {
                AvailableLevels.Add(fileName);
            }

            LevelInfo info = new LevelInfo(jsonPath);
            levelCleared = info.ClearedLevel();

            LevelInfos_[fileName] = info;
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
        int index = AvailableLevels.FindIndex((string level) =>
        {
            return level == currentLevel;
        });
        Debug.Assert(index != -1);
        index = Mathf.Clamp(index + 1, 0, AvailableLevels.Count - 1);
        return AvailableLevels[index];
    }
}
