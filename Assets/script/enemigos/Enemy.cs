using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Salud / Vida")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Efectos y Audio")]
    public GameObject deathEffectPrefab;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;

    // VARIABLE DE CONTROL PARA EVITAR MULTIPLES MUERTES
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        // CONFIRMA QUE ESTE MUERTO
        if (isDead) return;

        currentHealth -= damage;

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRed());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die()
    {
        // LO MARCA COMO MUERTO
        isDead = true;

        // DESACTIVA EL COLLIDER
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        // REGISTRA LA BAJA PARA EL GAME MANAGER
        if (GameManager.instance != null)
        {
            GameManager.instance.RegisterKill();
        }

        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject);
    }
}