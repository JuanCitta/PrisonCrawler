using UnityEngine;

public enum AbilityType { RapidFire, SpellBomb }

[CreateAssetMenu(fileName = "NewAbility", menuName = "PrisonCrawler/AbilityData")]
public class AbilityData : ScriptableObject
{
    [Header("Identificação")]
    public string abilityName = "Ability";
    public AbilityType abilityType;

    [Header("Cooldown (compartilhado)")]
    public float abilityCooldown = 10f;

    [Header("RapidFire — só preencha se abilityType = RapidFire")]
    [Tooltip("Duração do frenesi de tiro em segundos")]
    public float rapidFireDuration = 2f;
    [Tooltip("Multiplicador do cooldown durante o frenesi (0.3 = 30% do normal = 3x mais rápido)")]
    public float rapidFireCooldownMultiplier = 0.3f;

    [Header("SpellBomb — só preencha se abilityType = SpellBomb")]
    public GameObject spellBombProjectilePrefab;
    public int   spellBombProjectileCount  = 8;
    public int   spellBombDamage           = 1;
    public float spellBombProjectileSpeed  = 6f;
    [Tooltip("Alcance: lifetime × speed unidades")]
    public float spellBombProjectileLifetime = 1.5f;

    [Header("Inventário")]
    [Tooltip("Ícone que aparece no slot do inventário ao coletar esta habilidade. Arraste o sprite aqui.")]
    public Sprite inventoryIcon;
}
