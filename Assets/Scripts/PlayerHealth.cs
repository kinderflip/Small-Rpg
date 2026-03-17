using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private Animator animator;
    private PlayerMovement playerMovement;

    public Slider healthBar;

    private bool isDead = false;
    private GameOverUI gameOverUI;

    void Start()
    {
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Cache GameOverUI once (better than Find every time)
        gameOverUI = FindFirstObjectByType<GameOverUI>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        //Prevent taking damage after death
        if (isDead) return;

        currentHealth -= amount;

        if (healthBar != null)
            StartCoroutine(SmoothHealth());

        Debug.Log("Player took damage! Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return; //prevent double trigger
        isDead = true;

        Debug.Log("Player died!");

        if (animator != null)
        {
            animator.SetBool("isDead", true);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Delay before showing Game Over
        Invoke(nameof(ShowGameOver), 1.5f);
    }

    void ShowGameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
    }

    IEnumerator SmoothHealth()
    {
        float t = 0;
        float start = healthBar.value;

        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            healthBar.value = Mathf.Lerp(start, currentHealth, t);
            yield return null;
        }
    }
}