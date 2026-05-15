using System.Collections;
using UnityEngine;

public class CampRoomManager : MonoBehaviour
{
    IEnumerator Start()
    {
        // Espera um frame para garantir que o PlayerHealth já inicializou
        yield return null;

        if (PlayerHealth.Instance != null)
            PlayerHealth.Instance.Heal(1);
        else
            Debug.LogWarning("[CampRoom] PlayerHealth não encontrado.");

        foreach (var door in FindObjectsOfType<Door>())
            door.Unlock();
    }
}
