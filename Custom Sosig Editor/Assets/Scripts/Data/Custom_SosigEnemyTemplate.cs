using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_SosigEnemyTemplate
    {
        public string displayName = "New Sosig";
        public int sosigEnemyCategory = 0;
        public int sosigEnemyID = -1;

        public string[] weaponOptionsID;
        public string[] weaponOptions_SecondaryID;
        public float secondaryChance = 0;
        public string[] weaponOptions_TertiaryID;
        public float tertiaryChance = 0;

        public Custom_Sosig[] customSosig;
        public Custom_OutfitConfig[] outfitConfig;
        public Custom_SosigConfigTemplate[] configTemplates;
    }
}