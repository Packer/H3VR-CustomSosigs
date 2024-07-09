using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CustomSosigLoader
{
    public class OutfitConfigUI : MonoBehaviour
    {
        public Custom_OutfitConfig outfitConfig;

        public InputField[] headwearID;
        public InputField chance_HeadWear;
        public Toggle headUsesTorsoIndex;

        public InputField[] eyewearID;
        public InputField chance_Eyewear;

        public InputField[] torsowearID;
        public InputField chance_Torsowear;

        public InputField[] pantswearID;
        public InputField chance_Pantswear;
        public Toggle pantsUsesTorsoIndex;

        public InputField[] pantswear_LowerID;
        public InputField chance_Pantswear_Lower;
        public Toggle pantsLowerUsesPantsIndex;

        public InputField[] backpacksID;
        public InputField chance_Backpacks;

        public InputField[] torsoDecorationID;
        public InputField chance_TorsoDecoration;

        public InputField[] beltID;
        public InputField chance_belt;


        public void OpenOutfit(Custom_OutfitConfig outfit)
        {
            outfitConfig = outfit;


        }

    }
}