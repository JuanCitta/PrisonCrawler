using UnityEngine;

public class HealthUI : MonoBehaviour
{
    public GameObject[] hearts;

    void OnEnable()  => PlayerHealth.OnHealthChanged += UpdateHearts;
    void OnDisable() => PlayerHealth.OnHealthChanged -= UpdateHearts;

    void Start()
    {
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
