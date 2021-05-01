using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo
{
    [SerializeField]
    private int mBestTime;

    [SerializeField]
    private int mTimesPlayed;

    private string mJsonPath;

    public LevelInfo(string jsonPath)
    {
        mJsonPath = jsonPath;
        mBestTime = int.MaxValue;
        mTimesPlayed = 0;
        Load();
    }

    public void SetTime(int time)
    {
        if (time < mBestTime)
        {
            mBestTime = time;
            Save();
        }
    }

    public void IncrementTimesPlayed()
    {
        ++mTimesPlayed;
        Save();
    }

    public bool ClearedLevel()
    {
        return mBestTime != int.MaxValue;
    }

    private void Load()
    {
        Debug.Log("Loading...");
        Debug.Assert(File.Exists(mJsonPath));
        JsonUtility.FromJsonOverwrite(File.ReadAllText(mJsonPath), this);
        Debug.Log(string.Format("Loaded '{0}' with Time: {1}, Times: {2}", mJsonPath, mBestTime, mTimesPlayed));
    }

    private void Save()
    {
        Debug.Log("Saving...");
        Debug.Assert(File.Exists(mJsonPath));
        File.WriteAllText(mJsonPath, JsonUtility.ToJson(this));
        Debug.Log(string.Format("Saved '{0}' with Time: {1}, Times: {2}", mJsonPath, mBestTime, mTimesPlayed));
        Debug.Log("JsonData: " + JsonUtility.ToJson(this));
    }
}
