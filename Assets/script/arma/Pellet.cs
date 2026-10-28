using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 1.5f;
    public int damage = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.right * speed;
        }
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. IGNORA AL PLAYER
        if (collision.CompareTag("Player")) return;

        // 2. BUSCA OBJETOS COMO EL ENEMY
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject); // DESTRUIR LAS BALAS TRAS HACER DAÑO
            return;
        }

        // SI CHOCA CON ALGO QUE NO SEA TRIGGER SE DESTRUYE
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}