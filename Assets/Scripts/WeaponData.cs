using UnityEngine;

/// <summary>
/// Crie via: Assets > Create > PrisonCrawler > WeaponData
/// Cada arma tem seu próprio asset com stats independentes.
/// </summary>
[CreateAssetMenu(fileName = "NewWeapon", menuName = "PrisonCrawler/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Identificação")]
    public string weaponName = "Bow";

    [Header("Projétil")]
    public GameObject projectilePrefab;
    public int   damage             = 1;
    public float projectileSpeed    = 8f;
    /// <summary>
    /// Quanto tempo o projétil dura antes de se destruir.
    /// Alcance efetivo ≈ projectileLifetime × projectileSpeed unidades.
    /// </summary>
    public float projectileLifetime = 3f;

    [Header("Cadência")]
    public float shootCooldown = 0.25f;

    [Header("Inventário")]
    [Tooltip("Ícone que aparece no slot do inventário ao coletar esta arma. Arraste o sprite aqui.")]
    public Sprite inventoryIcon;
}
