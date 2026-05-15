using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;

    [Tooltip("Quantos slots de arma criar (aparecem primeiro)")]
    public int weaponSlotCount  = 1;
    [Tooltip("Quantos slots de habilidade criar (aparecem depois)")]
    public int abilitySlotCount = 1;

    void Start()
    {
        // Cria slots de arma
        for (int i = 0; i < weaponSlotCount; i++)
        {
            var go   = Instantiate(slotPrefab, inventoryPanel.transform);
            var slot = go.GetComponent<Slot>();
            if (slot != null) slot.slotType = SlotType.Weapon;
        }

        // Cria slots de habilidade
        for (int i = 0; i < abilitySlotCount; i++)
        {
            var go   = Instantiate(slotPrefab, inventoryPanel.transform);
            var slot = go.GetComponent<Slot>();
            if (slot != null) slot.slotType = SlotType.Ability;
        }

        // Restaura itens já coletados nesta run
        foreach (var (icon, type) in InventoryManager.Instance.GetItems())
            AddIcon(icon, type);
    }

    void OnEnable()
    {
        InventoryManager.OnItemAdded   += AddIcon;
        InventoryManager.OnWeaponForged += UpdateForgeIndicator;
    }

    void OnDisable()
    {
        InventoryManager.OnItemAdded   -= AddIcon;
        InventoryManager.OnWeaponForged -= UpdateForgeIndicator;
    }

    void AddIcon(Sprite icon, SlotType type)
    {
        // Procura slot vazio do tipo certo; se não houver, usa o primeiro do tipo (troca)
        Slot target = null;
        foreach (Transform t in inventoryPanel.transform)
        {
            Slot slot = t.GetComponent<Slot>();
            if (slot == null || slot.slotType != type) continue;

            if (slot.currentItem == null) { target = slot; break; }  // slot vazio — ideal
            if (target == null) target = slot;                        // fallback: substitui
        }

        if (target == null) { Debug.Log($"[Inventory] Nenhum slot de {type}."); return; }

        // Remove ícone anterior (troca de item)
        if (target.currentItem != null)
            Destroy(target.currentItem);

        // Cria novo ícone
        var go   = new GameObject(icon.name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(target.transform, false);

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = new Vector2(6, 6);
        rect.offsetMax = new Vector2(-6, -6);

        var img            = go.GetComponent<Image>();
        img.sprite         = icon;
        img.preserveAspect = true;

        target.currentItem = go;
    }

    // ── Indicador de forja ───────────────────────────────────────────────────

    void UpdateForgeIndicator(int level)
    {
        foreach (Transform t in inventoryPanel.transform)
        {
            Slot slot = t.GetComponent<Slot>();
            if (slot == null || slot.slotType != SlotType.Weapon) continue;

            // Procura ou cria o label no canto do slot
            var existing = t.Find("ForgeLabel");
            TextMeshProUGUI txt;

            if (existing != null)
            {
                txt = existing.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                var labelGo = new GameObject("ForgeLabel", typeof(RectTransform), typeof(TextMeshProUGUI));
                labelGo.transform.SetParent(t, false);

                var rect              = labelGo.GetComponent<RectTransform>();
                rect.anchorMin        = new Vector2(1f, 0f);   // canto inferior direito
                rect.anchorMax        = new Vector2(1f, 0f);
                rect.pivot            = new Vector2(1f, 0f);
                rect.anchoredPosition = new Vector2(-2f, 2f);
                rect.sizeDelta        = new Vector2(30f, 16f);

                txt           = labelGo.GetComponent<TextMeshProUGUI>();
                txt.fontSize  = 10;
                txt.alignment = TextAlignmentOptions.BottomRight;
                txt.color     = new Color(1f, 0.85f, 0.1f);    // dourado
            }

            txt.text = level > 0 ? $"▲{level}" : "";      // ▲1, ▲2...
            return;
        }
    }
}
