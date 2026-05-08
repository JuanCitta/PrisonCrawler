using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotcount;
    public GameObject [] itemPrefabs;

    void Start()
    {
        // Cria os slots vazios — itens são adicionados via WeaponPickup/AddItem
        for (int i = 0; i < slotcount; i++)
            Instantiate(slotPrefab, inventoryPanel.transform);
    }

    public bool AddItem(GameObject itemPrefab)
    {
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if(slot != null && slot.currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slot.transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory full");
        return false;
    }

    void Update()
    {
        
    }
}
