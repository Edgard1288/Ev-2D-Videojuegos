using UnityEngine;

public class GhostEnemy : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform playerTransform; // Se buscará automáticamente si no se asigna

    [Header("Detección y Movimiento")]
    public float chaseDistance = 8f;   // Distancia a la que empieza a perseguir
    public float moveSpeed = 2.5f;     // Velocidad de movimiento

    [Header("Efecto de Flotación")]
    public float floatAmplitude = 0.3f; // Qué tan alto y bajo flota
    public float floatFrequency = 2f;   // Qué tan rápido flota

    private SpriteRenderer spriteRenderer;
    private float startY;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startY = transform.position.y;

        // Si no asignaste el Player manualmente, buscarlo por Tag
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // EFECTO FLOTACION
        float newY = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;

        // 2. COMPROBAR QUE EL PLAYER ESTE DENTRO DEL RANGO
        if (distanceToPlayer <= chaseDistance)
        {
            // PARA PERSEGUIR AL PLAYER
            Vector3 targetPosition = playerTransform.position;
            
            // QUE SE MUEVA SIN QUE PIERDA LA ANIMACION DE FLOTACION 
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPosition + new Vector3(0f, newY, 0f), 
                moveSpeed * Time.deltaTime
            );

            // VOLTEAR SEGUN QUE DIRECCION ESTE MIRANDO 
            if (spriteRenderer != null)
            {
                if (playerTransform.position.x < transform.position.x)
                {
                    spriteRenderer.flipX = false; // IZQUIERDA
                }
                else if (playerTransform.position.x > transform.position.x)
                {
                    spriteRenderer.flipX = true;  // DERECHA
                }
            }
        }
    
    }

    // RANGO DE VISTA
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}