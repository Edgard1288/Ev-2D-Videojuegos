using System.Collections;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 10f;

    [Header("Doble Salto")]
    public int maxJumps = 2; // 2 = salto normal + salto en el aire
    private int remainingJumps;

    [Header("Knockback (Empuje por daño)")]
    public float knockbackForceX = 6f;
    public float knockbackForceY = 4f;
    public float knockbackDuration = 0.2f;
    private bool isKnockedBack;

    [Header("Salud / Vida")]
    public int maxHealth = 6; // 6 puntos = 3 corazones
    public int currentHealth;
    public HeartHUDManager heartHUD;

    [Header("Audio y Suelo")]
    public AudioClip jumpSound;
    public AudioClip hitSound; // Clip opcional para el sonido de daño
    public LayerMask groundLayer;
    public float extraRaycastDistance = 0.05f;

    private Rigidbody2D rb;
    private Collider2D playerCollider;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private Shotgun shotgunScript;

    private float moveInput;
    private bool jumpRequested;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        shotgunScript = GetComponentInChildren<Shotgun>();

        remainingJumps = maxJumps;

        currentHealth = maxHealth;
        if (heartHUD != null)
        {
            heartHUD.UpdateHearts(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        if (isKnockedBack) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        // salto
        if (Input.GetButtonDown("Jump") && remainingJumps > 0)
        {
            jumpRequested = true;
            remainingJumps--;

            if (audioSource != null && jumpSound != null)
            {
                audioSource.PlayOneShot(jumpSound);
            }

            if (animator != null)
            {
                animator.ResetTrigger("doJump");
                animator.SetTrigger("doJump");
                animator.Play("jump", -1, 0f);
            }
        }

        // Animator
        if (animator != null)
        {
            animator.SetBool("isRunning", moveInput != 0);
            animator.SetBool("isGrounded", isGrounded);
            animator.SetFloat("velocityY", rb.linearVelocity.y);
        }

        // Para voltear el personaje
        if (moveInput > 0) // DERECHA
        {
            spriteRenderer.flipX = true;
            if (shotgunScript != null) shotgunScript.SetDirection(true);
        }
        else if (moveInput < 0) // IZQUIERDA
        {
            spriteRenderer.flipX = false;
            if (shotgunScript != null) shotgunScript.SetDirection(false);
        }

    }

    void FixedUpdate()
    {
        CheckIfGrounded();

        if (!isKnockedBack)
        {
            rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);

            if (jumpRequested)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpRequested = false;
            }
        }
    }

    private void CheckIfGrounded() // para ver si esta tocando el suelo
    {
        if (playerCollider != null)
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(
                playerCollider.bounds.center,
                playerCollider.bounds.size,
                0f,
                Vector2.down,
                extraRaycastDistance,
                groundLayer
            );

            isGrounded = raycastHit.collider != null;

            if (isGrounded)
            {
                remainingJumps = maxJumps;
            }
        }
    }

    public void TakeDamage(int amount) // para recibir daño 
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (heartHUD != null)
        {
            heartHUD.UpdateHearts(currentHealth, maxHealth);
        }

        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (animator != null)
        {
            animator.ResetTrigger("doHit");
            animator.SetTrigger("doHit");
        }

        if (currentHealth > 0)
        {
            StartCoroutine(ApplyKnockback());
        }
        else
        {
            Debug.Log("¡El personaje ha muerto!");
            
            // para la pantalla de game over
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    private IEnumerator ApplyKnockback() // un empuje para cuando reciba daño
    {
        isKnockedBack = true;

        float direction = spriteRenderer.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * knockbackForceX, knockbackForceY);

        yield return new WaitForSeconds(knockbackDuration);

        isKnockedBack = false;
    }

    public void Heal(int amount) // esta es la vida del personaje 
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (heartHUD != null)
        {
            heartHUD.UpdateHearts(currentHealth, maxHealth);
        }
    }
}