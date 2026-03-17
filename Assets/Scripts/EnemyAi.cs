using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;

    public float detectionRange = 5f;
    public float speed = 2f;
    public float stopDistance = 0.8f;

    public Transform patrolPointA;
    public Transform patrolPointB;
    public float waitTime = 1.5f;

    private float waitCounter = 0f;
    private bool isWaiting = false;

    private Vector3 currentTarget;
    private bool playerDetected = false;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        currentTarget = patrolPointB.position;

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        
        if (animator.GetBool("IsAttacking"))
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
            playerDetected = true;
        else if (distance > detectionRange + 1f)
            playerDetected = false;

        if (playerDetected)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            animator.SetFloat("Xinput", dir.x);
            animator.SetFloat("Yinput", dir.y);

            if (distance <= stopDistance)
            {
                animator.SetFloat("Speed", 0f);
                return;
            }

            MoveTowards(player.position);
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            animator.SetFloat("Speed", 0f);

            if (waitCounter <= 0)
                isWaiting = false;

            return;
        }

        Vector2 direction = (currentTarget - transform.position).normalized;

        MoveTowards(currentTarget);
        SetAnimation(direction);

        if (Vector2.Distance(transform.position, currentTarget) < 0.1f)
        {
            isWaiting = true;
            waitCounter = waitTime;

            currentTarget = currentTarget == patrolPointA.position
                ? patrolPointB.position
                : patrolPointA.position;
        }
    }

    private void MoveTowards(Vector3 target)
    {
        Vector2 direction = (target - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        if (direction.x < 0)
            spriteRenderer.flipX = true;
        else if (direction.x > 0)
            spriteRenderer.flipX = false;
    }

    void SetAnimation(Vector2 direction)
    {
        animator.SetFloat("Xinput", direction.x);
        animator.SetFloat("Yinput", direction.y);
        animator.SetFloat("Speed", direction.magnitude);
    }
}