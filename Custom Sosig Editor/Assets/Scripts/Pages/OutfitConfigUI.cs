using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitConfigUI : MonoBehaviour
{
    public static OutfitConfigUI instance;
    public Custom_OutfitConfig outfitConfig;
    public InputField nameInputField;
    public Wear[] wears = new Wear[8];

    [System.Serializable]
    public class Wear
    {
        public string name;
        public WearType type = WearType.Head;
        [HideInInspector]
        public List<GenericButton> buttons;
        public Transform content;
        [HideInInspector]
        public InputField chance;
        public Toggle toggle;

        public void ClearButtons()
        {
            foreach (GenericButton button in buttons)
            {
                Destroy(button.gameObject);
            }
            buttons.Clear();
        }
    }

    public void Awake()
    {
        instance = this;
    }

    void ClearOutfit()
    {
        for (int i = 0; i < wears.Length; i++)
        {
            wears[i].ClearButtons();
        }
    }

    public void AddAccessory(int type)
    {
        GenericButton button = Global.SetupCollectionButton("", ItemType.Accessories, wears[(int)type].content);
        wears[(int)type].buttons.Add(button);

        
        switch (type)
        {
            default:
            case (int)WearType.Head:
                button.id = 0;
                //outfitConfig.headwearID.Add("");
                break;
            case (int)WearType.Eye:
                button.id = 1;
                //outfitConfig.eyewearID.Add("");
                break;
            case (int)WearType.Torso:
                button.id = 2;
                //outfitConfig.torsowearID.Add("");
                break;
            case (int)WearType.Pants:
                button.id = 3;
                //outfitConfig.pantswearID.Add("");
                break;
            case (int)WearType.PantsLower:
                button.id = 4;
                //outfitConfig.pantswear_LowerID.Add("");
                break;
            case (int)WearType.Backpacks:
                button.id = 5;
                //outfitConfig.backpacksID.Add("");
                break;
            case (int)WearType.TorsoDecoration:
                button.id = 6;
                //outfitConfig.torsoDecorationID.Add("");
                break;
            case (int)WearType.Belt:
                button.id = 7;
                //outfitConfig.beltID.Add("");
                break;
        }
        
    }

    public void RemoveAccessory(GenericButton button)
    {
        //index = Button index
        //id = WearType

        wears[button.id].buttons.Remove(button);
        /*
        switch ((WearType)button.id)
        {
            default:
            case WearType.Head:
                outfitConfig.headwearID.RemoveAt(button.index);
                break;
            case WearType.Eye:
                outfitConfig.eyewearID.RemoveAt(button.index);
                break;
            case WearType.Torso:
                outfitConfig.torsowearID.RemoveAt(button.index);
                break;
            case WearType.Pants:
                outfitConfig.pantswearID.RemoveAt(button.index);
                break;
            case WearType.PantsLower:
                outfitConfig.pantswear_LowerID.RemoveAt(button.index);
                break;
            case WearType.Backpacks:
                outfitConfig.backpacksID.RemoveAt(button.index);
                break;
            case WearType.TorsoDecoration:
                outfitConfig.torsoDecorationID.RemoveAt(button.index);
                break;
            case WearType.Belt:
                outfitConfig.beltID.RemoveAt(button.index);
                break;
        }
        */

        //Destroy Old Button
        Destroy(button);
    }

    public void ReloadOutfit()
    {
        SaveOutfit();
        OpenOutfit(outfitConfig);
    }

    public void OpenOutfit(Custom_OutfitConfig outfit)
    {
        //Clear old buttons
        ClearOutfit();

        outfitConfig = outfit;

        nameInputField.SetTextWithoutNotify(outfit.name);

        //Setup new Buttons for IDs
        wears[0].buttons = Global.SetupCollection(outfit.headwearID, ItemType.Accessories, wears[0].content);
        wears[1].buttons = Global.SetupCollection(outfit.eyewearID, ItemType.Accessories, wears[1].content);
        wears[2].buttons = Global.SetupCollection(outfit.torsowearID, ItemType.Accessories, wears[2].content);
        wears[3].buttons = Global.SetupCollection(outfit.pantswearID, ItemType.Accessories, wears[3].content);
        wears[4].buttons = Global.SetupCollection(outfit.pantswear_LowerID, ItemType.Accessories, wears[4].content);
        wears[5].buttons = Global.SetupCollection(outfit.backpacksID, ItemType.Accessories, wears[5].content);
        wears[6].buttons = Global.SetupCollection(outfit.torsoDecorationID, ItemType.Accessories, wears[6].content);
        wears[7].buttons = Global.SetupCollection(outfit.beltID, ItemType.Accessories, wears[7].content);

        //Setup Chance
        wears[0].chance.SetTextWithoutNotify(outfit.chance_HeadWear.ToString());
        wears[1].chance.SetTextWithoutNotify(outfit.chance_Eyewear.ToString());
        wears[2].chance.SetTextWithoutNotify(outfit.chance_Torsowear.ToString());
        wears[3].chance.SetTextWithoutNotify(outfit.chance_Pantswear.ToString());
        wears[4].chance.SetTextWithoutNotify(outfit.chance_Pantswear_Lower.ToString());
        wears[5].chance.SetTextWithoutNotify(outfit.chance_Backpacks.ToString());
        wears[6].chance.SetTextWithoutNotify(outfit.chance_TorsoDecoration.ToString());
        wears[7].chance.SetTextWithoutNotify(outfit.chance_belt.ToString());

        //Setup TorsoIndex Toggle
        wears[(int)WearType.Head].toggle.SetIsOnWithoutNotify(outfit.headUsesTorsoIndex);
        wears[(int)WearType.Pants].toggle.SetIsOnWithoutNotify(outfit.pantsUsesTorsoIndex);
        wears[(int)WearType.PantsLower].toggle.SetIsOnWithoutNotify(outfit.pantsLowerUsesPantsIndex);
    }

    public void SaveOutfit()
    {
        //Name
        outfitConfig.name = nameInputField.text;

        //Head
        outfitConfig.chance_HeadWear = float.Parse(wears[0].chance.text);
        outfitConfig.headUsesTorsoIndex = wears[0].toggle.isOn;
        outfitConfig.headwearID = Global.GenericButtonsToStringList(wears[0].buttons.ToArray());

        //Eye
        outfitConfig.chance_Eyewear = float.Parse(wears[1].chance.text);
        outfitConfig.eyewearID = Global.GenericButtonsToStringList(wears[1].buttons.ToArray());

        //Torso
        outfitConfig.chance_Torsowear = float.Parse(wears[2].chance.text);
        outfitConfig.torsowearID = Global.GenericButtonsToStringList(wears[2].buttons.ToArray());

        //Pants
        outfitConfig.chance_Pantswear = float.Parse(wears[3].chance.text);
        outfitConfig.pantsUsesTorsoIndex = wears[3].toggle.isOn;
        outfitConfig.pantswearID = Global.GenericButtonsToStringList(wears[3].buttons.ToArray());

        //Pants Lower
        outfitConfig.chance_Pantswear_Lower = float.Parse(wears[4].chance.text);
        outfitConfig.pantsLowerUsesPantsIndex = wears[4].toggle.isOn;
        outfitConfig.pantswear_LowerID = Global.GenericButtonsToStringList(wears[4].buttons.ToArray());

        //Backpacks
        outfitConfig.chance_Backpacks = float.Parse(wears[5].chance.text);
        outfitConfig.backpacksID = Global.GenericButtonsToStringList(wears[5].buttons.ToArray());

        //Torso Deco
        outfitConfig.chance_TorsoDecoration = float.Parse(wears[6].chance.text);
        outfitConfig.torsoDecorationID = Global.GenericButtonsToStringList(wears[6].buttons.ToArray());

        //Belt
        outfitConfig.chance_belt = float.Parse(wears[7].chance.text);
        outfitConfig.beltID = Global.GenericButtonsToStringList(wears[7].buttons.ToArray());
    }
}

public enum WearType
{
    Head = 0,
    Eye = 1,
    Torso = 2,
    Pants = 3,
    PantsLower = 4,
    Backpacks = 5,
    TorsoDecoration = 6,
    Belt = 7,
}