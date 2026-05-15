using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomDialogueUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI npcNameText;
    public TextMeshProUGUI dialogueText;
    public float displayDuration = 3f;

    // ── Falas por NPC ────────────────────────────────────────────────────────
    private static readonly Dictionary<string, string[]> Lines = new()
    {
        {
            "Knight", new[]
            {
                "Você não vai sair daqui com vida, prisioneiro.",
                "Eu já derrubei guerreiros muito melhores que você.",
                "Prepare-se para encontrar seu fim aqui.",
                "Honra? Aqui dentro não existe isso.",
                "Sua fuga termina agora."
            }
        },
        {
            "Archer", new[]
            {
                "Nem vai me ver antes de cair.",
                "Fuja o quanto quiser. Minha flecha sempre acha o alvo.",
                "Você é um alvo fácil demais.",
                "Cada passo seu já está calculado.",
                "Não existe esconderijo nessa sala."
            }
        },
        {
            "Mage", new[]
            {
                "Seu destino já foi escrito nas estrelas... e é curto.",
                "A magia não mente. Você não vai sobreviver.",
                "Os espíritos desta caverna me obedecem.",
                "Você deveria ter ficado na sua cela.",
                "Toda resistência é inútil contra o arcano."
            }
        }
    };

    // ── Cores por NPC ────────────────────────────────────────────────────────
    private static readonly Dictionary<string, Color> NPCColors = new()
    {
        { "Knight", new Color(1f,   0.45f, 0.2f)  }, // laranja
        { "Archer", new Color(0.3f, 0.9f,  0.3f)  }, // verde
        { "Mage",   new Color(0.65f,0.3f,  1f)    }, // roxo
    };

    void Start()
    {
        // Diálogo desativado
        gameObject.SetActive(false);
    }
}
