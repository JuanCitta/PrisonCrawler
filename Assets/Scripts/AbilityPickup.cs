using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    [Tooltip("Asset AbilityData desta habilidade")]
    public AbilityData abilityData;

    [Tooltip("Se falso, o pickup permanece no chão (usado no StartRoom para permitir troca)")]
    public bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (abilityData == null)
        {
            Debug.LogWarning("[AbilityPickup] abilityData não atribuído!", this);
            return;
        }

        PlayerAbility ability = other.GetComponent<PlayerAbility>();
        if (ability != null)
            ability.Equip(abilityData);

        // Adiciona ícone ao slot de habilidade
        if (abilityData.inventoryIcon != null)
            InventoryManager.Instance.AddItem(abilityData.inventoryIcon, SlotType.Ability);

        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
