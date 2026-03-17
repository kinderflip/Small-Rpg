using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public PlayerAttack playerAttack;

    [Header("References")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private InputSystem_Actions inputActions;
    private Vector2 moveInput;

    private Vector2 lastMoveDirection = Vector2.down;

    // ⭐ THIS FIXES YOUR ERROR
    public Vector2 LastMoveDirection => lastMoveDirection;

    void Awake()
    {
        inputActions = new InputSystem_Actions();

        if (playerAttack == null)
            playerAttack = GetComponent<PlayerAttack>();

        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += _ => moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void FixedUpdate()
    {
        if (rb == null || animator == null || spriteRenderer == null)
        {
            Debug.LogError("PlayerMovement: Missing component reference!");
            return;
        }

        Vector2 movement = moveInput.normalized;

        if (movement != Vector2.zero)
        {
            lastMoveDirection = movement;

            if (playerAttack != null)
                playerAttack.SetLastMoveDirection(movement);
        }

        spriteRenderer.flipX = lastMoveDirection.x < 0;

        animator.SetFloat("Xinput", lastMoveDirection.x);
        animator.SetFloat("Yinput", lastMoveDirection.y);
        animator.SetFloat("Speed", movement.magnitude);

        rb.linearVelocity = movement * speed;
    }
}