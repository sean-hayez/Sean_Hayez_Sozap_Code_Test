using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    private GameObject mPlayer;
    private GameObject[] mWalls;
    private GameObject[] mBoxes;
    private List<BoxHolderScript> mBoxHolderScripts;

    private HUDScript mHUDScript;

    private float mAnimationTime = 0.2f;
    private bool mIsMoving = false;

    void Awake()
    {
        mPlayer = GameObject.FindGameObjectWithTag("Player");
        mWalls = GameObject.FindGameObjectsWithTag("Wall");
        mBoxes = GameObject.FindGameObjectsWithTag("Box");

        mBoxHolderScripts = new List<BoxHolderScript>();
        var boxHolders = GameObject.FindGameObjectsWithTag("Box Holder");
        foreach (var boxHolder in boxHolders)
        {
            mBoxHolderScripts.Add(boxHolder.GetComponent<BoxHolderScript>());
        }

        var hudPrefab = Resources.Load("Prefabs/HUD") as GameObject;
        var hudGameObject = Instantiate(hudPrefab);
        mHUDScript = hudGameObject.GetComponentInChildren<HUDScript>();
    }

    // http://answers.unity.com/answers/1146980/view.html
    IEnumerator MoveOverSeconds(GameObject gameObject, Vector3 end, float seconds)
    {
        mIsMoving = true;
        float elapsedTime = 0;
        Vector3 startingPos = gameObject.transform.position;
        while (elapsedTime < seconds)
        {
            gameObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        end.x = Mathf.Round(end.x);
        end.y = Mathf.Round(end.y);
        gameObject.transform.position = end;
        mIsMoving = false;
    }

    bool CanMove(Vector3 nextPosition)
    {
        return Array.Find(mWalls, (GameObject go) =>
        {
            return go.transform.position == nextPosition;
        }) == null;
    }

    bool CanBoxMove(Vector3 nextPosition)
    {
        return Array.Find(mBoxes, (GameObject go) =>
        {
            return go.transform.position == nextPosition;
        }) == null;
    }

    bool CanPlayerMove(Vector3 direction)
    {
        return CanMove(mPlayer.transform.position + direction);
    }

    bool TryMoveBox(Vector3 direction)
    {
        var nextPlayerPosition = mPlayer.transform.position + direction;
        var box = Array.Find(mBoxes, (GameObject go) =>
        {
            return go.transform.position == nextPlayerPosition;
        });
        if (box)
        {
            var nextBoxPosition = box.transform.position + direction;
            if (!CanMove(nextBoxPosition) || !CanBoxMove(nextBoxPosition))
            {
                return false;
            }
            StartCoroutine(MoveOverSeconds(box, box.transform.position + direction, mAnimationTime));
        }
        return true;
    }

    void TryMove(Vector3 direction, params KeyCode[] keyCodes)
    {
        foreach (var keyCode in keyCodes)
        {
            if (!Input.GetKey(keyCode))
            {
                continue;
            }
            if (!CanPlayerMove(direction))
            {
                continue;
            }
            if (!TryMoveBox(direction))
            {
                continue;
            }
            StartCoroutine(MoveOverSeconds(mPlayer, mPlayer.transform.position + direction, mAnimationTime));
        }
    }

    bool DidCompleteLevel()
    {
        bool completedLevel = true;
        foreach (var boxHolder in mBoxHolderScripts)
        {
            completedLevel &= boxHolder.IsOverlapping(mBoxes);
        }
        return completedLevel;
    }

    void Update()
    {
        if (DidCompleteLevel())
        {
            mHUDScript.LevelCompleted();
            return;
        }

        if (!mIsMoving)
        {
            TryMove(Vector3.up, KeyCode.W, KeyCode.UpArrow);
            TryMove(Vector3.down, KeyCode.S, KeyCode.DownArrow);
            TryMove(Vector3.left, KeyCode.A, KeyCode.LeftArrow);
            TryMove(Vector3.right, KeyCode.D, KeyCode.RightArrow);
        }
    }
}
