using System.Collections;
using UnityEngine;

// Adicione este componente em qualquer GameObject com SpriteRenderer
// para obter um flash vermelho ao tomar dano.
public class DamageFlash : MonoBehaviour
{
    public Color flashColor    = Color.red;
    public float flashDuration = 0.12f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine activeFlash;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }

    public void Flash()
    {
        if (sr == null) return;
        if (activeFlash != null) StopCoroutine(activeFlash);
        activeFlash = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
        activeFlash = null;
    }
}
