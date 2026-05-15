using UnityEngine;

/// <summary>
/// Coloca este script no GameObject "Decor" (dentro de Cave ou Castle).
/// Cada filho do Decor tem X% de chance de aparecer.
/// </summary>
public class DecorRandomizer : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("Probabilidade de cada objeto de decor aparecer")]
    public float chanceDeAparecer = 0.6f;

    void Awake()
    {
        foreach (Transform filho in transform)
            filho.gameObject.SetActive(Random.value < chanceDeAparecer);
    }
}
