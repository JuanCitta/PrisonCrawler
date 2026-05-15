using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("InventoryManager");
                _instance = go.AddComponent<InventoryManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Cada entrada guarda o sprite e o tipo de slot que deve ocupar
    private readonly List<(Sprite icon, SlotType type)> collectedItems = new();

    public static event System.Action<Sprite, SlotType> OnItemAdded;

    // ── Forja ────────────────────────────────────────────────────────────────
    public int weaponForgeLevel { get; private set; } = 0;
    public static event System.Action<int> OnWeaponForged;

    void Awake()
    {
        if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); }
        else if (_instance != this) Destroy(gameObject);
    }

    public void AddItem(Sprite icon, SlotType type)
    {
        if (icon == null) return;

        // Se já existe um item deste tipo, substitui em vez de acumular
        int existing = collectedItems.FindIndex(e => e.type == type);
        if (existing >= 0)
            collectedItems[existing] = (icon, type);
        else
            collectedItems.Add((icon, type));

        OnItemAdded?.Invoke(icon, type);
    }

    public IReadOnlyList<(Sprite icon, SlotType type)> GetItems() => collectedItems.AsReadOnly();

    public void ApplyForge()
    {
        weaponForgeLevel++;
        OnWeaponForged?.Invoke(weaponForgeLevel);
    }

    public void Reset()
    {
        collectedItems.Clear();
        weaponForgeLevel = 0;
    }
}
