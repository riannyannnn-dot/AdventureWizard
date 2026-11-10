using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

// Attach this to the Wizard.
// WASD (or the arrow keys) moves the player in eight directions and swaps
// the sprite so the wizard faces the way it is walking.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Facing sprites")]
    public Sprite down;
    public Sprite downLeft;
    public Sprite left;
    public Sprite upLeft;
    public Sprite up;
    public Sprite upRight;
    public Sprite right;
    public Sprite downRight;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Top-down game: no gravity, and never let collisions spin the wizard.
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = ReadInput();

        if (moveInput != Vector2.zero)
        {
            Sprite facing = SpriteFor(moveInput);
            if (facing != null) spriteRenderer.sprite = facing;
        }
    }

    void FixedUpdate()
    {
        // Normalize so diagonal movement isn't faster than straight movement.
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private Vector2 ReadInput()
    {
        float x = 0f;
        float y = 0f;

#if ENABLE_INPUT_SYSTEM
        Keyboard kb = Keyboard.current;
        if (kb == null) return Vector2.zero;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) y += 1f;
#else
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) x -= 1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) x += 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) y -= 1f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) y += 1f;
#endif

        return new Vector2(x, y);
    }

    private Sprite SpriteFor(Vector2 dir)
    {
        if (dir.y > 0f)
        {
            if (dir.x < 0f) return upLeft;
            if (dir.x > 0f) return upRight;
            return up;
        }

        if (dir.y < 0f)
        {
            if (dir.x < 0f) return downLeft;
            if (dir.x > 0f) return downRight;
            return down;
        }

        return dir.x < 0f ? left : right;
    }
}
