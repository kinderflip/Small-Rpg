using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 1.2f;
    public float attackCooldown = 1f;
    public Transform player;

    private float cooldownTimer = 0f;

    private Animator animator;
    private PlayerHealth playerHealth;

    private bool hasDealtDamage = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null || playerHealth == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Face player
        Vector2 direction = (player.position - transform.position).normalized;
        animator.SetFloat("Xinput", direction.x);
        animator.SetFloat("Yinput", direction.y);

        // If currently attacking → do nothing
        if (animator.GetBool("IsAttacking"))
            return;

        // Trigger attack
        if (distance <= attackRange && cooldownTimer <= 0f)
        {
            animator.SetFloat("Speed", 0f);
            animator.SetBool("IsAttacking", true);

            cooldownTimer = attackCooldown;
        }

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    //Called at HIT FRAME (animation event)
    public void DealDamage()
    {
        if (!hasDealtDamage && playerHealth != null)
        {
            playerHealth.TakeDamage(1);
            hasDealtDamage = true;
        }
    }

    // Called at END of animation (animation event)
    public void ResetAttack()
    {
        animator.SetBool("IsAttacking", false);
        hasDealtDamage = false;
    }
}