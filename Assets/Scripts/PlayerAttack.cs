using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public PlayerMovement playerMovement;

    [Header("Slash Prefabs")]
    public GameObject slashEffectSide;
    public GameObject slashEffectUp;
    public GameObject slashEffectDown;

    [Header("Spawn Points")]
    public Transform downSlashSpawn;
    public Transform topSlashSpawn;
    public Transform sideSlashSpawn;

    public float attackCooldown = 0.3f;

    private float attackTimer;
    private bool isAttacking = false;
    private Vector2 lastMoveDirection = Vector2.down;

    void Update()
    {
        attackTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && attackTimer <= 0 && !isAttacking)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        animator.SetBool("isAttacking", true);
        rb.linearVelocity = Vector2.zero;

        PlaySlashEffect();

        attackTimer = attackCooldown;

        playerMovement.enabled = false;

        yield return new WaitForSeconds(attackCooldown);

        playerMovement.enabled = true;
        animator.SetBool("isAttacking", false);

        isAttacking = false;
    }

    public void PlaySlashEffect()
    {
        GameObject prefabToSpawn = slashEffectSide;
        Transform spawnPoint = sideSlashSpawn;
        bool flip = false;

        if (lastMoveDirection.y > 0.5f)
        {
            prefabToSpawn = slashEffectUp;
            spawnPoint = topSlashSpawn;
        }
        else if (lastMoveDirection.y < -0.5f)
        {
            prefabToSpawn = slashEffectDown;
            spawnPoint = downSlashSpawn;
        }
        else if (lastMoveDirection.x > 0.5f)
        {
            prefabToSpawn = slashEffectSide;
            spawnPoint = sideSlashSpawn;
            flip = false;
        }
        else if (lastMoveDirection.x < -0.5f)
        {
            prefabToSpawn = slashEffectSide;
            spawnPoint = sideSlashSpawn;
            flip = true;
        }

        GameObject slash = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

        SpriteRenderer sr = slash.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.flipX = flip;
        }

        Destroy(slash, 0.3f);
    }

    public void SetLastMoveDirection(Vector2 dir)
    {
        if (dir != Vector2.zero)
        {
            lastMoveDirection = dir;
        }
    }
}