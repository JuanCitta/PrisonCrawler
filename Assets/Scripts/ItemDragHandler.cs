using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour
{
    Transform originalParent;
    CanvasGroup canvasGroup;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
