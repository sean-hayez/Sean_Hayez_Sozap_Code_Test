using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelFixerTool : MonoBehaviour
{
    static List<GameObject> GetAllLevelObjects()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.Add(GameObject.FindGameObjectWithTag("Player"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Wall"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Box"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("Box Holder"));
        return gameObjects;
    }

    static void FixTransforms()
    {
        var gameObjects = GetAllLevelObjects();
        foreach (var gameObject in gameObjects)
        {
            var t = gameObject.transform;
            var position = t.position;
            position.x = (float)Math.Round(position.x);
            position.y = (float)Math.Round(position.y);
            position.z = 0;
            t.position = position;
            t.rotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
    }

    static void CenterCameraOnLevel()
    {
        GameObject cameraGameObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (!cameraGameObject)
        {
            Debug.LogError("Could not find a camera with the tag 'MainCamera'");
            return;
        }

        Bounds levelBounds = new Bounds();
        var gameObjects = GetAllLevelObjects();
        foreach (var gameObject in gameObjects)
        {
            levelBounds.Encapsulate(gameObject.GetComponent<Renderer>().bounds);
        }

        Camera camera = cameraGameObject.GetComponent<Camera>();
        Vector3 cameraPosition = camera.transform.position;
        cameraPosition = levelBounds.center;
        cameraPosition.z = -10;
        float newSize = Mathf.Max(levelBounds.size.x, levelBounds.size.y);

        camera.transform.position = cameraPosition;
        camera.orthographic = true;
        camera.orthographicSize = newSize / 2;
    }

    [MenuItem("Sozap/Fix Level")]
    static void FixLevel()
    {
        FixTransforms();
        CenterCameraOnLevel();
    }
}
