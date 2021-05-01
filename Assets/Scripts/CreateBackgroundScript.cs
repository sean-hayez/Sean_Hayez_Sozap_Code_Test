using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CreateBackgroundScript : MonoBehaviour
{
    void Awake()
    {
        var backgroundPrefab = Resources.Load("Prefabs/Background") as GameObject;
        var backgroundGameObject = Instantiate(backgroundPrefab);
        backgroundGameObject.transform.parent = gameObject.transform;

        var camera = GetComponent<Camera>();
        float tileX = camera.orthographicSize * 2.0f;
        float tileY = tileX / Screen.height * Screen.width;

        backgroundGameObject.transform.localPosition = new Vector3(-tileX / 2, tileY / 2, 1);

        var spriteRenderer = backgroundGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -1;
        spriteRenderer.size = new Vector2(tileX, tileY);
    }
}
