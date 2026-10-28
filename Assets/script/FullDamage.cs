using UnityEngine; // SOLO AUMENTE EL DAÑO PARA TENER UN LIMITE

public class FullDamage : MonoBehaviour
{
    public int damageAmount = 999; // 1 punto = medio corazón

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Intenta obtener el script PlayerController2 del objeto que chocó
        PlayerController2 player = collision.gameObject.GetComponent<PlayerController2>();

        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController2 player = collision.GetComponent<PlayerController2>();

        if (player != null)
        {
            player.TakeDamage(damageAmount);
        }
    }
}