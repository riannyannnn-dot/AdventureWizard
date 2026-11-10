using UnityEngine;

// Loops through a list of sprites to animate simple props (torches, coins, chests).
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimator : MonoBehaviour
{
    public Sprite[] frames;
    public float framesPerSecond = 6f;
    public bool randomizeStart = true; // stops every torch flickering in sync

    private SpriteRenderer spriteRenderer;
    private float timer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (randomizeStart && frames != null && frames.Length > 0 && framesPerSecond > 0f)
        {
            timer = Random.Range(0f, frames.Length / framesPerSecond);
        }
    }

    void Update()
    {
        if (frames == null || frames.Length == 0 || framesPerSecond <= 0f) return;

        timer += Time.deltaTime;
        int index = (int)(timer * framesPerSecond) % frames.Length;
        if (frames[index] != null) spriteRenderer.sprite = frames[index];
    }
}
