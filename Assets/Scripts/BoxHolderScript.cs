using System;
using UnityEngine;

public class BoxHolderScript : MonoBehaviour
{
    private GameObject mTrail;

    void Awake()
    {
        var trailTransform = transform.Find("Trail");
        mTrail = trailTransform.gameObject;
        mTrail.SetActive(false);
    }

    public bool IsOverlapping(GameObject[] gameObjects)
    {
        var found = Array.Find(gameObjects, (GameObject gameObject) =>
        {
            return gameObject.transform.position == transform.position;
        });
        mTrail.SetActive(found != null);
        return found != null;
    }
}
