using UnityEngine;
using UnityEngine.UI;

public class HeartHUDManager : MonoBehaviour
{
    [Header("Sprites de Corazón")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Contenedores en el UI")]
    public Image[] heartImages;

    public void UpdateHearts(int currentHealth, int maxHealth)
    {
        // Validación para evitar NullReferenceException
        if (heartImages == null || heartImages.Length == 0)
        {
            Debug.LogWarning("La lista Heart Images está vacía en el inspector de HeartHUDManager.");
            return;
        }

        for (int i = 0; i < heartImages.Length; i++)
        {
            // Salta elementos no asignados
            if (heartImages[i] == null) continue;

            int heartValue = (i + 1) * 2;

            if (currentHealth >= heartValue)
            {
                heartImages[i].sprite = fullHeart;
                heartImages[i].enabled = true;
            }
            else if (currentHealth == heartValue - 1)
            {
                heartImages[i].sprite = halfHeart;
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].sprite = emptyHeart;
                heartImages[i].enabled = true;
            }

            if (i >= maxHealth / 2)
            {
                heartImages[i].enabled = false;
            }
        }
    }
}