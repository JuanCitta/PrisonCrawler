using System.Collections;
using UnityEngine;

/// <summary>
/// Congela o jogo por alguns milissegundos ao acertar um inimigo.
/// Coloque este componente num GameObject da CombatRoom (junto com o CombatManager, por exemplo).
/// </summary>
public class HitPause : MonoBehaviour
{
    public static HitPause Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    /// <summary>Pausa o jogo por 'duration' segundos em tempo real.</summary>
    public void Pause(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(PauseRoutine(duration));
    }

    IEnumerator PauseRoutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}
