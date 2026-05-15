using UnityEngine;

public class BiomeVisuals : MonoBehaviour
{
    void Awake()
    {
        bool isCastle = GameManager.Instance != null &&
                        GameManager.Instance.currentBiome == BiomeType.Castle;

        // Encontra pelos nomes — não precisa de referências no Inspector
        GameObject cave   = GameObject.Find("Cave");
        GameObject castle = GameObject.Find("Castle");

        if (cave   != null) cave  .SetActive(!isCastle);
        if (castle != null) castle.SetActive(isCastle);
    }
}
