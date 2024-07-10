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
        public List<GenericButton> buttons;
        public Transform content;
        [HideInInspector]
        public InputField chance;
        public Toggle toggle;

        public void ClearButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i])
                    Destroy(buttons[i].gameObject);
                Debug.Log("A " + i);
            }
            buttons.Clear();
        }

        public void SetButtons(List<GenericButton> newButtons)
        {
            for (int i = 0; i < newButtons.Count; i++)
            {
                newButtons[i].id = (int)type;
                newButtons[i].image.sprite = Global.GetSpriteByName(ItemType.Accessories, newButtons[i].inputField.text);
            }
            buttons = newButtons;
        }
    }

    public void Awake()
    {
        instance = this;
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
        //SaveOutfit();
    }

    public void RemoveAccessory(GenericButton button)
    {
        //id - WearType
        Debug.Log(button.id);
        wears[button.id].buttons.Remove(button);

        //Destroy Old Button
        Destroy(button.gameObject);

        SaveOutfit();
    }

    public void ReloadOutfit()
    {
        SaveOutfit();
        OpenOutfit(outfitConfig);
    }


    //----------------------------------------------------------------------------
    // Load Save Clear
    //----------------------------------------------------------------------------

    void ClearOutfit()
    {
        for (int i = 0; i < wears.Length; i++)
        {
            wears[i].ClearButtons();
        }
    }

    public void OpenOutfit(Custom_OutfitConfig outfit)
    {
        //Clear old buttons
        ClearOutfit();

        outfitConfig = outfit;

        SosigEnemyTemplateUI.instance.outfitTitleText.text = "OUTFIT CONFIG: " + outfit.name;
        nameInputField.SetTextWithoutNotify(outfit.name);

        //Setup new Buttons for IDs
        wears[0].SetButtons(Global.SetupCollection(outfit.headwearID, ItemType.Accessories, wears[0].content));
        wears[1].SetButtons(Global.SetupCollection(outfit.eyewearID, ItemType.Accessories, wears[1].content));
        wears[2].SetButtons(Global.SetupCollection(outfit.torsowearID, ItemType.Accessories, wears[2].content));
        wears[3].SetButtons(Global.SetupCollection(outfit.pantswearID, ItemType.Accessories, wears[3].content));
        wears[4].SetButtons(Global.SetupCollection(outfit.pantswear_LowerID, ItemType.Accessories, wears[4].content));
        wears[5].SetButtons(Global.SetupCollection(outfit.backpacksID, ItemType.Accessories, wears[5].content));
        wears[6].SetButtons(Global.SetupCollection(outfit.torsoDecorationID, ItemType.Accessories, wears[6].content));
        wears[7].SetButtons(Global.SetupCollection(outfit.beltID, ItemType.Accessories, wears[7].content));

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
        outfitConfig.chance_HeadWear = Mathf.Clamp01(float.Parse(wears[0].chance.text));
        outfitConfig.headUsesTorsoIndex = wears[0].toggle.isOn;
        outfitConfig.headwearID = Global.GenericButtonsToStringList(wears[0].buttons.ToArray());

        //Eye
        outfitConfig.chance_Eyewear = Mathf.Clamp01(float.Parse(wears[1].chance.text));
        outfitConfig.eyewearID = Global.GenericButtonsToStringList(wears[1].buttons.ToArray());

        //Torso
        outfitConfig.chance_Torsowear = Mathf.Clamp01(float.Parse(wears[2].chance.text));
        outfitConfig.torsowearID = Global.GenericButtonsToStringList(wears[2].buttons.ToArray());

        //Pants
        outfitConfig.chance_Pantswear = Mathf.Clamp01(float.Parse(wears[3].chance.text));
        outfitConfig.pantsUsesTorsoIndex = wears[3].toggle.isOn;
        outfitConfig.pantswearID = Global.GenericButtonsToStringList(wears[3].buttons.ToArray());

        //Pants Lower
        outfitConfig.chance_Pantswear_Lower = Mathf.Clamp01(float.Parse(wears[4].chance.text));
        outfitConfig.pantsLowerUsesPantsIndex = wears[4].toggle.isOn;
        outfitConfig.pantswear_LowerID = Global.GenericButtonsToStringList(wears[4].buttons.ToArray());

        //Backpacks
        outfitConfig.chance_Backpacks = Mathf.Clamp01(float.Parse(wears[5].chance.text));
        outfitConfig.backpacksID = Global.GenericButtonsToStringList(wears[5].buttons.ToArray());

        //Torso Deco
        outfitConfig.chance_TorsoDecoration = Mathf.Clamp01(float.Parse(wears[6].chance.text));
        outfitConfig.torsoDecorationID = Global.GenericButtonsToStringList(wears[6].buttons.ToArray());

        //Belt
        outfitConfig.chance_belt = Mathf.Clamp01(float.Parse(wears[7].chance.text));
        outfitConfig.beltID = Global.GenericButtonsToStringList(wears[7].buttons.ToArray());

        //Save Log
        ManagerUI.Log("Outfit saved at: " + System.DateTime.Now);

        //Update UI
        SosigEnemyTemplateUI.instance.OutfitLoad();
    }

    //----------------------------------------------------------------------------
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