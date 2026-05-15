using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            bool opening = !menuCanvas.activeSelf;
            menuCanvas.SetActive(opening);

            // Atualiza os stats sempre que o menu é aberto
            if (opening)
                FindObjectOfType<StatsUI>()?.Refresh();
        }
    }
}
