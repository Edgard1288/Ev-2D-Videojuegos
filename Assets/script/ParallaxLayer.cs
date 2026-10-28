using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Referencias")]
    public Transform cam;

    [Header("Configuración Parallax")]
    [Tooltip("0 = Se mueve pegado a la cámara. 0.5 = Mitad de velocidad. 1 = Estático en el escenario.")]
    [Range(0f, 1f)]
    public float parallaxEffect = 0.5f;

    private float startPosX;
    private float lengthX;

    void Start()
    {
        if (cam == null && Camera.main != null)
        {
            cam = Camera.main.transform;
        }

        startPosX = transform.position.x;

        // Mide el ancho de uno de los Sprites hijos
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            lengthX = sr.bounds.size.x;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        // Posicionamiento desfasado según el efecto Parallax
        float temp = (cam.position.x * (1 - parallaxEffect));
        float distance = (cam.position.x * parallaxEffect);

        transform.position = new Vector3(startPosX + distance, transform.position.y, transform.position.z);

        // Al tener 3 imágenes, el salto ocurre antes de que la pantalla toque el borde
        if (lengthX > 0)
        {
            if (temp > startPosX + lengthX)
            {
                startPosX += lengthX;
            }
            else if (temp < startPosX - lengthX)
            {
                startPosX -= lengthX;
            }
        }
    }
}