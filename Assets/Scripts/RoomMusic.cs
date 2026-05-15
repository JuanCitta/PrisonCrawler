using UnityEngine;

/// <summary>
/// Coloca este script num GameObject de cada cena.
/// Toca automaticamente a música correta conforme o bioma/sala.
/// </summary>
public class RoomMusic : MonoBehaviour
{
    public enum MusicType { Auto, Start, Cave, Castle, Boss }

    [Tooltip("Auto = decide pelo bioma. Usa Manual para forçar uma música específica.")]
    public MusicType musicType = MusicType.Auto;

    void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("[RoomMusic] AudioManager não encontrado. Adiciona-o à StartRoom.");
            return;
        }

        AudioClip clip = GetClip();
        if (clip == null)
        {
            Debug.LogWarning("[RoomMusic] AudioClip não atribuído no AudioManager.");
            return;
        }

        AudioManager.Instance.PlayMusic(clip);
    }

    AudioClip GetClip()
    {
        var am = AudioManager.Instance;

        switch (musicType)
        {
            case MusicType.Start:  return am.musicaStart;
            case MusicType.Cave:   return am.musicaCave;
            case MusicType.Castle: return am.musicaCastle;
            case MusicType.Boss:   return am.musicaBoss;
            default: // Auto
                if (GameManager.Instance == null) return am.musicaCave;
                int floor = GameManager.Instance.currentFloor;
                if (floor == 8 || floor == 16) return am.musicaBoss;
                return GameManager.Instance.currentBiome == BiomeType.Castle
                    ? am.musicaCastle
                    : am.musicaCave;
        }
    }
}
