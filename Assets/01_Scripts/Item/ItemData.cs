using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
    Craft
}

public enum ConsumableType
{
    Health,
    Hunger
}

public enum CraftType
{
    Wood,
    Stone,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[Serializable]
public class ItemDataCraft
{
    public CraftType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefabs;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmout;

    [Header("Consumable")]
    public ItemDataConsumable[] cosumables;

    [Header("CraftType")]
    public ItemDataCraft[] crafts;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Debug")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
}
