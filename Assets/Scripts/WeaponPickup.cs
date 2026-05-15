using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [Tooltip("Asset WeaponData desta arma (crie via Assets > Create > PrisonCrawler > WeaponData)")]
    public WeaponData weaponData;

    [Tooltip("Se falso, o pickup permanece no chão (usado no StartRoom para permitir troca)")]
    public bool destroyOnPickup = true;

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

        // Adiciona ícone ao slot de arma
        if (weaponData.inventoryIcon != null)
            InventoryManager.Instance.AddItem(weaponData.inventoryIcon, SlotType.Weapon);

        // Remove o item do mundo (se não for StartRoom)
        if (destroyOnPickup)
            Destroy(gameObject);
    }
}
