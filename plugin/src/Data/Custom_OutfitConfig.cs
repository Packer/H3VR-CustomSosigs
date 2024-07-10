using UnityEngine;
using FistVR;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_OutfitConfig
    {
        public SosigOutfitConfig Initialize()
        {
            SosigOutfitConfig outfit = ScriptableObject.CreateInstance<SosigOutfitConfig>();

            outfit.HeadUsesTorsoIndex = headUsesTorsoIndex;
            outfit.PantsUsesTorsoIndex = pantsUsesTorsoIndex;
            outfit.PantsLowerUsesPantsIndex = pantsLowerUsesPantsIndex;

            outfit.Headwear = new List<FVRObject>();
            Global.ItemIDToList(headwearID, outfit.Headwear);
            outfit.Eyewear = new List<FVRObject>();
            Global.ItemIDToList(eyewearID, outfit.Eyewear);
            outfit.Torsowear = new List<FVRObject>();
            Global.ItemIDToList(torsowearID, outfit.Torsowear);
            outfit.Pantswear = new List<FVRObject>();
            Global.ItemIDToList(pantswearID, outfit.Pantswear);
            outfit.Pantswear_Lower = new List<FVRObject>();
            Global.ItemIDToList(pantswear_LowerID, outfit.Pantswear_Lower);
            outfit.Backpacks = new List<FVRObject>();
            Global.ItemIDToList(backpacksID, outfit.Backpacks);
            outfit.TorosDecoration = new List<FVRObject>();
            Global.ItemIDToList(torsoDecorationID, outfit.TorosDecoration);
            outfit.Belt = new List<FVRObject>();
            Global.ItemIDToList(beltID, outfit.Belt);

            outfit.Chance_Headwear = chance_HeadWear;
            outfit.Chance_Eyewear = chance_Eyewear;
            outfit.Chance_Torsowear = chance_Torsowear;
            outfit.Chance_Pantswear = chance_Pantswear;
            outfit.Chance_Pantswear_Lower = chance_Pantswear_Lower;
            outfit.Chance_Backpacks = chance_Backpacks;
            outfit.Chance_TorosDecoration = chance_TorsoDecoration;
            outfit.Chance_Belt = chance_belt;

            return outfit;
        }

        public string name;

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
