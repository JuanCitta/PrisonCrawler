using System.Collections;
using UnityEngine;
using TMPro;

// Como usar:
// 1. Crie um Panel no Canvas chamado "QuestNotification" (FORA do menuCanvas)
// 2. Adicione este script ao Panel
// 3. Adicione um TextMeshProUGUI filho e arraste para o campo NotificationText
// 4. O painel aparece automaticamente ao completar uma quest e some após 3 segundos
public class QuestNotificationUI : MonoBehaviour
{
    public TextMeshProUGUI notificationText;
    public float displayDuration = 3f;

    private Coroutine hideCoroutine;

    void OnEnable()
    {
        QuestManager.OnQuestCompleted += ShowNotification;
    }

    void OnDisable()
    {
        QuestManager.OnQuestCompleted -= ShowNotification;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    void ShowNotification(string npcId)
    {
        if (notificationText != null)
            notificationText.text = $"Quest completa: {npcId}!";

        gameObject.SetActive(true);

        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        hideCoroutine = StartCoroutine(HideRoutine());
    }

    IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(displayDuration);
        gameObject.SetActive(false);
    }
}
