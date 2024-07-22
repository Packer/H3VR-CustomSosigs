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
        public string DisplayName = "New Sosig";
        public int SosigEnemyCategory = 0;
        public int SosigEnemyID = -1;

        public Custom_Sosig[] CustomSosigs;
        public Custom_OutfitConfig[] OutfitConfigs;
        public Custom_SosigConfigTemplate[] Configs;
        public Custom_SosigConfigTemplate[] ConfigsEasy;

        public string[] WeaponOptions;
        public string[] WeaponOptionsSecondary;
        public string[] WeaponOptionsTertiary;
        public float SecondaryChance = 0;
        public float TertiaryChance = 0;

        // TnHFramework
        //public float DroppedLootChance = 0;
        //public LootTable[] DroppedObjectPool; TODO Reconstruct TnH Framework loot pool


        public SosigEnemyTemplate Initialize()
        {
            SosigEnemyTemplate template = ScriptableObject.CreateInstance<SosigEnemyTemplate>();

            //Outfits
            template.OutfitConfig = new List<SosigOutfitConfig>();
            for (int i = 0; i < OutfitConfigs.Length; i++)
            {
                template.OutfitConfig.Add(OutfitConfigs[i].Initialize());
            }

            //Configs & Prefabs
            template.ConfigTemplates = new List<SosigConfigTemplate>();
            for (int i = 0; i < Configs.Length; i++)
            {
                template.ConfigTemplates.Add(Configs[i].Initialize());
            }

            template.ConfigTemplates_Easy = new List<SosigConfigTemplate>();
            if (ConfigsEasy.Length > 0)
            {
                for (int i = 0; i < ConfigsEasy.Length; i++)
                {
                    template.ConfigTemplates.Add(ConfigsEasy[i].Initialize());
                }
            }
            else
                template.ConfigTemplates_Easy.AddRange(template.ConfigTemplates);

            //Custom Base
            template.SosigPrefabs = new List<FVRObject>();
            for (int i = 0; i < CustomSosigs.Length; i++)
            {
                SosigEnemyTemplate baseTemplate = IM.Instance.odicSosigObjsByID[CustomSosigs[i].baseSosigID];
                FVRObject newSosig = baseTemplate.SosigPrefabs[Random.Range(0, baseTemplate.SosigPrefabs.Count)];
                template.SosigPrefabs.Add(newSosig);
            }

            template.WeaponOptions = new List<FVRObject>();
            Global.ItemIDToList(WeaponOptions, template.WeaponOptions);
            template.WeaponOptions_Secondary = new List<FVRObject>();
            Global.ItemIDToList(WeaponOptionsSecondary, template.WeaponOptions_Secondary);
            template.WeaponOptions_Tertiary = new List<FVRObject>();
            Global.ItemIDToList(WeaponOptionsTertiary, template.WeaponOptions_Tertiary);

            template.SecondaryChance = SecondaryChance;
            template.TertiaryChance = TertiaryChance;

            return template;
        }

        public void ExportJson()
        {
            using (StreamWriter streamWriter = new StreamWriter(Paths.PluginPath + "\\Packer-SupplyRaid\\" + DisplayName + ".json"))
            {
                string json = JsonUtility.ToJson(this, true);
                streamWriter.Write(json);
            }
        }
    }
}