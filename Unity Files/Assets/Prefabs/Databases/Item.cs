using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemStruct[] existentItems;
}

[System.Serializable]
public class ItemStruct
{
    public int id;
    public string name;
    public Sprite uiSprite;
    public bool stackable;
    public GameObject groundItem;
    
    public EquipmentSettings equipmentSettings;
    public FoodSettings foodSettings;
    public DrinkSettings drinkSettings;
    public UsableSettings usableSettings;

    public WeaponSettings weaponSettings;
    public ToolSettings toolSettings;
}

[System.Serializable]
public class WeaponSettings
{
    public bool isWeapon;
    public float minDamage;
    public float maxDamage;
    public float durability;
    public GameObject prefab;
}

[System.Serializable]
public class ToolSettings
{
    public bool isTool;
    public float strength;
    public float durability;
    public int action;
    public GameObject prefab;
}

[System.Serializable]
public class EquipmentSettings
{
    public bool isEquipable;
}

[System.Serializable]
public class FoodSettings
{
    public bool isEatable;
    public float foodValue;
}

[System.Serializable]
public class DrinkSettings
{
    public bool isDrinkable;
    public float drinkValue;
}

[System.Serializable]
public class UsableSettings
{
    public bool isUsable;
}
