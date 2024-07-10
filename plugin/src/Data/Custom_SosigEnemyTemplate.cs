using UnityEngine;
using FistVR;
using System.IO;
using BepInEx;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_SosigEnemyTemplate
    {
        public string displayName = "New Sosig";
        public int sosigEnemyCategory = 0;
        public int sosigEnemyID = -1;
        public Custom_Sosig[] customSosig;

        public string[] weaponOptionsID;
        public string[] weaponOptions_SecondaryID;
        public float secondaryChance = 0;
        public string[] weaponOptions_TertiaryID;
        public float tertiaryChance = 0;

        public Custom_OutfitConfig[] outfitConfig;
        public Custom_SosigConfigTemplate[] configTemplates;

        public SosigEnemyTemplate Initialize()
        {
            SosigEnemyTemplate template = ScriptableObject.CreateInstance<SosigEnemyTemplate>();

            //Outfits
            template.OutfitConfig = new List<SosigOutfitConfig>();
            for (int i = 0; i < outfitConfig.Length; i++)
            {
                template.OutfitConfig.Add(outfitConfig[i].Initialize());
            }

            //Configs & Prefabs
            template.ConfigTemplates = new List<SosigConfigTemplate>();
            for (int i = 0; i < configTemplates.Length; i++)
            {
                template.ConfigTemplates.Add(configTemplates[i].Initialize());
            }

            //Custom Base
            template.SosigPrefabs = new List<FVRObject>();
            for (int i = 0; i < customSosig.Length; i++)
            {
                SosigEnemyTemplate baseTemplate = IM.Instance.odicSosigObjsByID[customSosig[i].baseSosigID];
                FVRObject newSosig = baseTemplate.SosigPrefabs[Random.Range(0, baseTemplate.SosigPrefabs.Count)];
                template.SosigPrefabs.Add(newSosig);
            }

            template.WeaponOptions = new List<FVRObject>();
            Global.ItemIDToList(weaponOptionsID, template.WeaponOptions);
            template.WeaponOptions_Secondary = new List<FVRObject>();
            Global.ItemIDToList(weaponOptions_SecondaryID, template.WeaponOptions_Secondary);
            template.WeaponOptions_Tertiary = new List<FVRObject>();
            Global.ItemIDToList(weaponOptions_TertiaryID, template.WeaponOptions_Tertiary);

            template.SecondaryChance = secondaryChance;
            template.TertiaryChance = tertiaryChance;

            return template;
        }

        public void ExportJson()
        {
            using (StreamWriter streamWriter = new StreamWriter(Paths.PluginPath + "\\Packer-SupplyRaid\\" + displayName + ".json"))
            {
                string json = JsonUtility.ToJson(this, true);
                streamWriter.Write(json);
            }
        }
    }
}