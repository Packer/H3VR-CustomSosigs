using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class Custom_SosigEnemyTemplate
{
    public string displayName = "Enemy Template";
    public int sosigEnemyCategory = 0;
    public int sosigEnemyID = -1;

    public List<string> weaponOptionsID = new List<string>();
    public List<string> weaponOptions_SecondaryID = new List<string>();
    public float secondaryChance = 0;
    public List<string> weaponOptions_TertiaryID = new List<string>();
    public float tertiaryChance = 0;

    public List<Custom_Sosig> customSosig = new List<Custom_Sosig>();
    public List<Custom_OutfitConfig> outfitConfig = new List<Custom_OutfitConfig>();
    public List<Custom_SosigConfigTemplate> configTemplates = new List<Custom_SosigConfigTemplate>();
}