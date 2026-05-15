using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Item")) return;

        Item item = collision.GetComponent<Item>();
        if (item == null) return;

        // Pega o sprite do SpriteRenderer do item no mundo
        SpriteRenderer sr = collision.GetComponent<SpriteRenderer>();
        if (sr == null) sr = collision.GetComponentInChildren<SpriteRenderer>();

        if (sr != null && sr.sprite != null)
            InventoryManager.Instance.AddItem(sr.sprite, SlotType.Weapon);

        Destroy(collision.gameObject);
    }
}
