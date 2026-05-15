using UnityEngine;

/// <summary>
/// Singleton persistente para música e SFX.
/// Coloca num GameObject da StartRoom — persiste entre cenas.
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource music;
    public AudioSource sfx;

    [Header("Músicas")]
    public AudioClip musicaCave;
    public AudioClip musicaCastle;
    public AudioClip musicaBoss;
    public AudioClip musicaStart;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || music.clip == clip) return;
        music.clip = clip;
        music.loop = true;
        music.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfx.PlayOneShot(clip);
    }

    public void StopMusic() => music.Stop();
}
