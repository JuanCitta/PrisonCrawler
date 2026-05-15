using System.Collections;
using UnityEngine;

public class ForgeRoomManager : MonoBehaviour
{
    IEnumerator Start()
    {
        // Espera um frame para garantir que o PlayerShoot já inicializou
        yield return null;

        var shoot = PlayerHealth.Instance?.GetComponent<PlayerShoot>();
        if (shoot != null)
        {
            shoot.forgeCooldownMultiplier *= 0.8f;
            int pct = Mathf.RoundToInt((1f - shoot.forgeCooldownMultiplier) * 100f);
            Debug.Log($"[ForgeRoom] Cadência melhorada. Total: +{pct}% mais rápido.");
        }
        else
        {
            Debug.LogWarning("[ForgeRoom] PlayerShoot não encontrado.");
        }

        InventoryManager.Instance.ApplyForge();

        foreach (var door in FindObjectsOfType<Door>())
            door.Unlock();
    }
}
