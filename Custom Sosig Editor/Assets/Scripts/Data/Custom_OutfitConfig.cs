using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Custom_OutfitConfig
{
    public string name = "Outfit";

    public List<string> Headwear = new List<string>();
    public float Chance_HeadWear = 0;
    public bool HeadUsesTorsoIndex = false;
    public bool ForceWearAllHead = false;

    public List<string> Eyewear = new List<string>();
    public float Chance_Eyewear = 0;
    public bool ForceWearAllEye = false;

    public List<string> Torsowear = new List<string>();
    public float Chance_Torsowear = 0;
    public bool ForceWearAllTorso = false;

    public List<string> Pantswear = new List<string>();
    public float Chance_Pantswear = 0;
    public bool PantsUsesTorsoIndex = false;
    public bool ForceWearAllPants = false;

    public List<string> Pantswear_Lower = new List<string>();
    public float Chance_Pantswear_Lower = 0;
    public bool PantsLowerUsesPantsIndex = false;
    public bool ForceWearAllPantsLower = false;

    public List<string> Backpacks = new List<string>();
    public float Chance_Backpacks = 0;
    public bool ForceWearAllBackpacks = false;

    public List<string> TorsoDecoration = new List<string>();
    public float Chance_TorsoDecoration = 0;
    public bool ForceWearAllTorsoDecoration = false;

    public List<string> Belt = new List<string>();
    public float Chance_belt = 0;
    public bool ForceWearAllBelt = false;

    public List<string> Facewear = new List<string>();
    public float Chance_Facewear = 0;
    public bool ForceWearAllFace = false;
}
