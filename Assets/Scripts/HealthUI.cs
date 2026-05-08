using UnityEngine;

// Como usar:
// 1. Crie um Canvas separado chamado "HUD" (não dentro do menuCanvas)
// 2. Adicione este script ao HUD
// 3. Crie 5 Image GameObjects representando corações e arraste-os para o array Hearts
public class HealthUI : MonoBehaviour
{
    public GameObject[] hearts;

    void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateHearts;
    }

    void OnDisable()
    {
        PlayerHealth.OnHealthChanged -= UpdateHearts;
    }

    void Start()
    {
        // Sincroniza com o valor atual ao abrir a cena
        if (PlayerHealth.Instance != null)
            UpdateHearts(PlayerHealth.Instance.currentLives);
    }

    void UpdateHearts(int currentLives)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < currentLives);
        }
    }
}
