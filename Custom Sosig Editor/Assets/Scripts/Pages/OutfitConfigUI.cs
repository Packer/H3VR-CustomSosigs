using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitConfigUI : MonoBehaviour
{
    public static OutfitConfigUI instance;
    public Custom_OutfitConfig outfitConfig;
    public InputField nameInputField;
    public Wear[] wears = new Wear[9];

    [System.Serializable]
    public class Wear
    {
        public string name;
        public WearType type = WearType.Head;
        public List<GenericButton> buttons;
        public Transform content;
        public InputField chance;
        public Toggle toggle;

        public void ClearButtons()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i])
                    Destroy(buttons[i].gameObject);
                //Debug.Log("A " + i);
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
                break;
            case (int)WearType.Eye:
                button.id = 1;
                break;
            case (int)WearType.Torso:
                button.id = 2;
                break;
            case (int)WearType.Pants:
                button.id = 3;
                break;
            case (int)WearType.PantsLower:
                button.id = 4;
                break;
            case (int)WearType.Backpacks:
                button.id = 5;
                break;
            case (int)WearType.TorsoDecoration:
                button.id = 6;
                break;
            case (int)WearType.Belt:
                button.id = 7;
                break;
            case (int)WearType.Face:
                button.id = 8;
                break;
        }
        //SaveOutfit();
    }

    public void RemoveAccessory(GenericButton button)
    {
        //id - WearType
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
        wears[0].SetButtons(Global.SetupCollection(outfit.Headwear, ItemType.Accessories, wears[0].content));
        wears[1].SetButtons(Global.SetupCollection(outfit.Eyewear, ItemType.Accessories, wears[1].content));
        wears[2].SetButtons(Global.SetupCollection(outfit.Torsowear, ItemType.Accessories, wears[2].content));
        wears[3].SetButtons(Global.SetupCollection(outfit.Pantswear, ItemType.Accessories, wears[3].content));
        wears[4].SetButtons(Global.SetupCollection(outfit.Pantswear_Lower, ItemType.Accessories, wears[4].content));
        wears[5].SetButtons(Global.SetupCollection(outfit.Backpacks, ItemType.Accessories, wears[5].content));
        wears[6].SetButtons(Global.SetupCollection(outfit.TorsoDecoration, ItemType.Accessories, wears[6].content));
        wears[7].SetButtons(Global.SetupCollection(outfit.Belt, ItemType.Accessories, wears[7].content));
        wears[8].SetButtons(Global.SetupCollection(outfit.Facewear, ItemType.Accessories, wears[8].content));

        //Setup Chance
        wears[0].chance.SetTextWithoutNotify(outfit.Chance_Headwear.ToString());
        wears[1].chance.SetTextWithoutNotify(outfit.Chance_Eyewear.ToString());
        wears[2].chance.SetTextWithoutNotify(outfit.Chance_Torsowear.ToString());
        wears[3].chance.SetTextWithoutNotify(outfit.Chance_Pantswear.ToString());
        wears[4].chance.SetTextWithoutNotify(outfit.Chance_Pantswear_Lower.ToString());
        wears[5].chance.SetTextWithoutNotify(outfit.Chance_Backpacks.ToString());
        wears[6].chance.SetTextWithoutNotify(outfit.Chance_TorsoDecoration.ToString());
        wears[7].chance.SetTextWithoutNotify(outfit.Chance_Belt.ToString());
        wears[8].chance.SetTextWithoutNotify(outfit.Chance_Facewear.ToString());

        //Setup TorsoIndex Toggle
        wears[(int)WearType.Head].toggle.SetIsOnWithoutNotify(outfit.HeadUsesTorsoIndex);
        wears[(int)WearType.Pants].toggle.SetIsOnWithoutNotify(outfit.PantsUsesTorsoIndex);
        wears[(int)WearType.PantsLower].toggle.SetIsOnWithoutNotify(outfit.PantsLowerUsesPantsIndex);
    }

    public void SaveOutfit()
    {
        //Name
        outfitConfig.name = nameInputField.text;

        //Head
        outfitConfig.Chance_Headwear = Mathf.Clamp01(float.Parse(wears[0].chance.text));
        outfitConfig.HeadUsesTorsoIndex = wears[0].toggle.isOn;
        outfitConfig.Headwear = Global.GenericButtonsToStringList(wears[0].buttons.ToArray());

        //Eye
        outfitConfig.Chance_Eyewear = Mathf.Clamp01(float.Parse(wears[1].chance.text));
        outfitConfig.Eyewear = Global.GenericButtonsToStringList(wears[1].buttons.ToArray());

        //Torso
        outfitConfig.Chance_Torsowear = Mathf.Clamp01(float.Parse(wears[2].chance.text));
        outfitConfig.Torsowear = Global.GenericButtonsToStringList(wears[2].buttons.ToArray());

        //Pants
        outfitConfig.Chance_Pantswear = Mathf.Clamp01(float.Parse(wears[3].chance.text));
        outfitConfig.PantsUsesTorsoIndex = wears[3].toggle.isOn;
        outfitConfig.Pantswear = Global.GenericButtonsToStringList(wears[3].buttons.ToArray());

        //Pants Lower
        outfitConfig.Chance_Pantswear_Lower = Mathf.Clamp01(float.Parse(wears[4].chance.text));
        outfitConfig.PantsLowerUsesPantsIndex = wears[4].toggle.isOn;
        outfitConfig.Pantswear_Lower = Global.GenericButtonsToStringList(wears[4].buttons.ToArray());

        //Backpacks
        outfitConfig.Chance_Backpacks = Mathf.Clamp01(float.Parse(wears[5].chance.text));
        outfitConfig.Backpacks = Global.GenericButtonsToStringList(wears[5].buttons.ToArray());

        //Torso Deco
        outfitConfig.Chance_TorsoDecoration = Mathf.Clamp01(float.Parse(wears[6].chance.text));
        outfitConfig.TorsoDecoration = Global.GenericButtonsToStringList(wears[6].buttons.ToArray());

        //Belt
        outfitConfig.Chance_Belt = Mathf.Clamp01(float.Parse(wears[7].chance.text));
        outfitConfig.Belt = Global.GenericButtonsToStringList(wears[7].buttons.ToArray());

        //Facewear
        outfitConfig.Chance_Facewear = Mathf.Clamp01(float.Parse(wears[8].chance.text));
        outfitConfig.Facewear = Global.GenericButtonsToStringList(wears[8].buttons.ToArray());

        //Save Log
        ManagerUI.Log("Outfit applied at: " + System.DateTime.Now);

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
    Face = 8,
}