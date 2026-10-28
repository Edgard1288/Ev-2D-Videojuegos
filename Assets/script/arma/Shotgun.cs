using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject pelletPrefab;
    public Transform firePoint;
    public int pelletsCount = 4;
    public float spreadAngle = 10f;
    public float fireRate = 0.4f;
    private float nextFireTime;

    [Header("Audio y Render")]
    public AudioClip shootSound;
    private AudioSource audioSource;
    private SpriteRenderer shotgunSprite;

    private Vector3 initialLocalPosition;
    private bool isFacingRight = true;

    void Start()
    {
        shotgunSprite = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Guardamos la posición relativa local inicial (ej: X = 0.3, Y = 0)
        initialLocalPosition = transform.localPosition;
    }

    void Update()
    {

        if (Time.timeScale == 0f) return;
        
        HandleAiming();

        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    // Este método lo llama el PlayerController2 cuando cambia de sentido
    public void SetDirection(bool facingRight)
    {
        isFacingRight = facingRight;
    }

    void HandleAiming()
    {
        if (Camera.main == null) return;

        // 1. Obtener la posición del personaje en coordenadas de pantalla
        Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        // 2. Vector dirección desde el personaje hacia el cursor
        Vector2 direction = Input.mousePosition - playerScreenPos;

        // 3. Ángulo exacto del cursor respecto al personaje en grados (-180° a 180°)
        float mouseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. Cambiar la posición X de la escopeta al lado correspondiente del cuerpo
        float xOffset = Mathf.Abs(initialLocalPosition.x);
        transform.localPosition = new Vector3(isFacingRight ? xOffset : -xOffset, initialLocalPosition.y, initialLocalPosition.z);

        // 5. Ángulo base: 0° (Frente derecha) o 180° (Frente izquierda)
        float baseAngle = isFacingRight ? 0f : 180f;

        // 6. Restringir estricta y únicamente a un abanico de 90° (-45° a +45°) respecto al frente
        float angleOffset = Mathf.DeltaAngle(baseAngle, mouseAngle);
        float clampedOffset = Mathf.Clamp(angleOffset, -45f, 45f);
        float finalAngle = baseAngle + clampedOffset;

        // 7. Aplicar la rotación final a la escopeta
        transform.rotation = Quaternion.Euler(0f, 0f, finalAngle);

        // 8. Voltear verticalmente la textura para que la escopeta no quede cabeza abajo al mirar a la izquierda
        if (shotgunSprite != null)
        {
            shotgunSprite.flipY = !isFacingRight;
        }
    }

    void Shoot()
    {
        if (pelletPrefab == null || firePoint == null) return;

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        for (int i = 0; i < pelletsCount; i++)
        {
            float randomSpread = Random.Range(-spreadAngle, spreadAngle);
            Quaternion pelletRotation = transform.rotation * Quaternion.Euler(0f, 0f, randomSpread);
            Instantiate(pelletPrefab, firePoint.position, pelletRotation);
        }
    }
}