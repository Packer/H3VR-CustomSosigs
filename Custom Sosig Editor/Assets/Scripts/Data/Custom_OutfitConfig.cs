using UnityEngine;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_OutfitConfig
    {
        public string[] headwearID;
        public float chance_HeadWear = 0;
        public bool headUsesTorsoIndex = false;

        public string[] eyewearID;
        public float chance_Eyewear = 0;

        public string[] torsowearID;
        public float chance_Torsowear = 0;

        public string[] pantswearID;
        public float chance_Pantswear = 0;
        public bool pantsUsesTorsoIndex = false;

        public string[] pantswear_LowerID;
        public float chance_Pantswear_Lower = 0;
        public bool pantsLowerUsesPantsIndex = false;

        public string[] backpacksID;
        public float chance_Backpacks = 0;

        public string[] torsoDecorationID;
        public float chance_TorsoDecoration = 0;

        public string[] beltID;
        public float chance_belt = 0;
    }
}
