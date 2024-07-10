using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Custom_OutfitConfig
{
    public string name = "Outfit";

    public List<string> headwearID = new List<string>();
    public float chance_HeadWear = 0;
    public bool headUsesTorsoIndex = false;

    public List<string> eyewearID = new List<string>();
    public float chance_Eyewear = 0;

    public List<string> torsowearID = new List<string>();
    public float chance_Torsowear = 0;

    public List<string> pantswearID = new List<string>();
    public float chance_Pantswear = 0;
    public bool pantsUsesTorsoIndex = false;

    public List<string> pantswear_LowerID = new List<string>();
    public float chance_Pantswear_Lower = 0;
    public bool pantsLowerUsesPantsIndex = false;

    public List<string> backpacksID = new List<string>();
    public float chance_Backpacks = 0;

    public List<string> torsoDecorationID = new List<string>();
    public float chance_TorsoDecoration = 0;

    public List<string> beltID = new List<string>();
    public float chance_belt = 0;
}
