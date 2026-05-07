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
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
