using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class Custom_SosigEnemyTemplate
{
    public string DisplayName = "Enemy Template";
    public int SosigEnemyCategory = 0;
    public int SosigEnemyID = -1;

    public List<Custom_Sosig> CustomSosigs = new List<Custom_Sosig>();
    public List<Custom_SosigConfigTemplate> Configs = new List<Custom_SosigConfigTemplate>();
    public List<Custom_SosigConfigTemplate> ConfigsEasy = new List<Custom_SosigConfigTemplate>();
    public List<Custom_OutfitConfig> OutfitConfigs = new List<Custom_OutfitConfig>();

    public List<string> WeaponOptions = new List<string>();
    public List<string> WeaponOptionsSecondary = new List<string>();
    public List<string> WeaponOptionsTertiary = new List<string>();
    public float SecondaryChance = 0;
    public float TertiaryChance = 0;

    // TnHFramework
    //public float DroppedLootChance = 0;
    //public LootTable[] DroppedObjectPool; //TODO ADD TNH LOOT TABLE TO THIS???

}