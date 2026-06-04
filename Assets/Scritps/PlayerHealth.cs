using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Death")]
    [SerializeField] private float deathAnimationTime = 1.0f;

    private int currentHealth;

    private Animator animator;
    private HeroKnight heroKnight;
    private Rigidbody2D rb;
    private Collider2D[] colliders;

    private bool isDead;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        heroKnight = GetComponent<HeroKnight>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<Collider2D>();

        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;

        Debug.Log($"Player HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
                animator.SetTrigger("Hurt");
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        Debug.Log("PLAYER DEAD");

        if (animator != null)
            animator.SetTrigger("Death");

        if (heroKnight != null)
            heroKnight.enabled = false;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        StartCoroutine(DisableAnimatorAfterDeath());
    }

    private IEnumerator DisableAnimatorAfterDeath()
    {
        yield return new WaitForSeconds(deathAnimationTime);

        if (animator != null)
            animator.enabled = false;
    }

    [ContextMenu("Kill Player")]
    private void KillPlayer()
    {
        TakeDamage(maxHealth);
    }
}