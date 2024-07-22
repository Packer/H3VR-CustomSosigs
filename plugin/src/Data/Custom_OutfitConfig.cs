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

            outfit.HeadUsesTorsoIndex = HeadUsesTorsoIndex;
            outfit.PantsUsesTorsoIndex = PantsUsesTorsoIndex;
            outfit.PantsLowerUsesPantsIndex = PantsLowerUsesPantsIndex;

            outfit.Headwear = new List<FVRObject>();
            Global.ItemIDToList(Headwear, outfit.Headwear);
            outfit.Eyewear = new List<FVRObject>();
            Global.ItemIDToList(Eyewear, outfit.Eyewear);
            outfit.Torsowear = new List<FVRObject>();
            Global.ItemIDToList(Torsowear, outfit.Torsowear);
            outfit.Pantswear = new List<FVRObject>();
            Global.ItemIDToList(Pantswear, outfit.Pantswear);
            outfit.Pantswear_Lower = new List<FVRObject>();
            Global.ItemIDToList(Pantswear_Lower, outfit.Pantswear_Lower);
            outfit.Backpacks = new List<FVRObject>();
            Global.ItemIDToList(Backpacks, outfit.Backpacks);
            outfit.TorosDecoration = new List<FVRObject>();
            Global.ItemIDToList(TorsoDecoration, outfit.TorosDecoration);
            outfit.Belt = new List<FVRObject>();
            Global.ItemIDToList(Belt, outfit.Belt);
            outfit.Facewear = new List<FVRObject>();
            Global.ItemIDToList(Facewear, outfit.Facewear);

            outfit.Chance_Headwear = Chance_Headwear;
            outfit.Chance_Eyewear = Chance_Eyewear;
            outfit.Chance_Torsowear = Chance_Torsowear;
            outfit.Chance_Pantswear = Chance_Pantswear;
            outfit.Chance_Pantswear_Lower = Chance_Pantswear_Lower;
            outfit.Chance_Backpacks = Chance_Backpacks;
            outfit.Chance_TorosDecoration = Chance_TorsoDecoration;
            outfit.Chance_Belt = Chance_belt;
            outfit.Chance_Facewear = Chance_Facewear;

            return outfit;
        }

        public string name;

        public string[] Headwear;
        public float Chance_Headwear = 0;
        public bool HeadUsesTorsoIndex = false;
        public bool ForceWearAllHead = false;

        public string[] Eyewear;
        public float Chance_Eyewear = 0;
        public bool ForceWearAllEye = false;

        public string[] Torsowear;
        public float Chance_Torsowear = 0;
        public bool ForceWearAllTorso = false;

        public string[] Pantswear;
        public float Chance_Pantswear = 0;
        public bool PantsUsesTorsoIndex = false;
        public bool ForceWearAllPants = false;

        public string[] Pantswear_Lower;
        public float Chance_Pantswear_Lower = 0;
        public bool PantsLowerUsesPantsIndex = false;
        public bool ForceWearAllPantsLower = false;

        public string[] Backpacks;
        public float Chance_Backpacks = 0;
        public bool ForceWearAllBackpacks = false;

        public string[] TorsoDecoration;
        public float Chance_TorsoDecoration = 0;
        public bool ForceWearAllTorsoDecoration = false;

        public string[] Belt;
        public float Chance_belt = 0;
        public bool ForceWearAllBelt = false;

        public string[] Facewear;
        public float Chance_Facewear = 0;
        public bool ForceWearAllFace = false;
    }
}
