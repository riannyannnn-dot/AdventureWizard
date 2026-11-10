using UnityEngine;

// Attach this to the Main Camera.
// Smoothly follows the target and keeps the view inside the map so the
// camera doesn't drift off into the empty space around the dungeon.
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.15f;

    [Header("Map bounds (world units)")]
    public Vector2 mapCenter = Vector2.zero;
    public Vector2 mapSize = new Vector2(35f, 19f);

    private Camera cam;
    private Vector3 velocity;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        if (target != null) transform.position = Clamp(target.position);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 goal = Clamp(target.position);
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }

    private Vector3 Clamp(Vector3 position)
    {
        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        // If the map is smaller than the view on an axis, just centre on it.
        float rangeX = Mathf.Max(0f, mapSize.x * 0.5f - halfWidth);
        float rangeY = Mathf.Max(0f, mapSize.y * 0.5f - halfHeight);

        float x = Mathf.Clamp(position.x, mapCenter.x - rangeX, mapCenter.x + rangeX);
        float y = Mathf.Clamp(position.y, mapCenter.y - rangeY, mapCenter.y + rangeY);

        return new Vector3(x, y, transform.position.z);
    }
}
