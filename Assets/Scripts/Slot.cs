using UnityEngine;

public enum SlotType { Weapon, Ability }

public class Slot : MonoBehaviour
{
    public SlotType     slotType    = SlotType.Weapon;
    public GameObject   currentItem;
}
