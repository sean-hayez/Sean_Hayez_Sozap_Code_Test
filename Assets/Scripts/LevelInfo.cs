using System;
using System.IO;
using UnityEngine;

public class LevelInfo
{
    [SerializeField]
    private int mBestTime;

    [SerializeField]
    private int mAttempts;

    private string mJsonPath;

    public LevelInfo(string jsonPath)
    {
        mJsonPath = jsonPath;
        mBestTime = int.MaxValue;
        mAttempts = 0;
        Load();
    }

    private void Load()
    {
        Debug.Assert(File.Exists(mJsonPath));
        JsonUtility.FromJsonOverwrite(File.ReadAllText(mJsonPath), this);
    }

    private void Save()
    {
        Debug.Assert(File.Exists(mJsonPath));
        File.WriteAllText(mJsonPath, JsonUtility.ToJson(this));
    }

    public void SetTime(int time)
    {
        if (time < mBestTime)
        {
            mBestTime = time;
            Save();
        }
    }

    public void IncrementAttempts()
    {
        ++mAttempts;
        Save();
    }

    public bool ClearedLevel()
    {
        return mBestTime != int.MaxValue;
    }

    public int GetAttempts()
    {
        return mAttempts;
    }

    public string GetBestTime()
    {
        TimeSpan duration = TimeSpan.FromSeconds(mBestTime);
        return duration.ToString(@"m\:s");
    }
}
