using UnityEngine;

/// <summary>
/// Coloque este script no GameObject do arco (ou qualquer arma) no mundo.
/// Requer: Collider2D com IsTrigger = true.
/// Quando o player tocar, equipa a arma e destrói o objeto do mundo.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Asset WeaponData desta arma (crie via Assets > Create > PrisonCrawler > WeaponData)")]
    public WeaponData weaponData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (weaponData == null)
        {
            Debug.LogWarning("[WeaponPickup] weaponData não atribuído!", this);
            return;
        }

        // Equipa no PlayerShoot
        PlayerShoot shoot = other.GetComponent<PlayerShoot>();
        if (shoot != null)
            shoot.Equip(weaponData);

        // Adiciona ao inventário (se houver prefab de UI configurado)
        if (weaponData.inventoryItemPrefab != null)
        {
            InventoryController inv = FindObjectOfType<InventoryController>();
            inv?.AddItem(weaponData.inventoryItemPrefab);
        }

        // Remove o item do mundo
        Destroy(gameObject);
    }
}
