using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Painel de instruções (você vai criar no passo 5)
    public GameObject painelInstrucoes;

    public void BotaoJogar()
    {
        SceneManager.LoadScene("StartRoom"); // nome exato da sua cena
    }

    public void BotaoInstrucoes()
    {
        painelInstrucoes.SetActive(true);
    }

    public void FecharInstrucoes()
    {
        painelInstrucoes.SetActive(false);
    }
}