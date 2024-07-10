using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SosigEnemyTemplateUI : MonoBehaviour
{
    public static SosigEnemyTemplateUI instance;

    public Custom_SosigEnemyTemplate template;

    [Header("Enemy Template")]
    public InputField displayName;
    public InputField sosigEnemyCategory;
    public Dropdown sosigEnemyCategoryDropdown;
    public InputField sosigEnemyID;

    public List<GenericButton> weaponOptionsID;
    [SerializeField] Transform primaryContent;

    public List<GenericButton> weaponOptions_SecondaryID;
    public InputField secondaryChance;
    [SerializeField] Transform secondaryContent;

    public List<GenericButton> weaponOptions_TertiaryID;
    [SerializeField] Transform tertiaryContent;
    public InputField tertiaryChance;

    [Header("Configs")]
    public Custom_Sosig[] customSosig;

    public Dropdown outfitDropdown;
    public Custom_OutfitConfig[] outfitConfig;

    public Custom_SosigConfigTemplate[] configTemplates;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Auto Populate
        PopulateEnemyCategoryDropdown();
    }

    //----------------------------------------------------------------------------
    // Outfit Config Menu
    //----------------------------------------------------------------------------

    public void OutfitLoad()
    {
        outfitDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < template.outfitConfig.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.outfitConfig[i].name;
            options.Add(option);
        }
        outfitDropdown.AddOptions(options);
    }

    public void OutfitAdd()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_OutfitConfig outfit = new Custom_OutfitConfig();
        option.text = outfit.name;
        template.outfitConfig.Add(outfit);
        outfitDropdown.options.Add(option);
    }

    public void OutfitDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_OutfitConfig outfit = template.outfitConfig[outfitDropdown.value];
        option.text = outfit.name;
        template.outfitConfig.Add(outfit);
        outfitDropdown.options.Add(option);
    }

    public void OutfitRemove()
    {
        //Don't delete last option
        if (outfitDropdown.options.Count <= 1)
            return;

        template.outfitConfig.RemoveAt(outfitDropdown.value);
        outfitDropdown.options.RemoveAt(outfitDropdown.value);
    }

    public void OutfitSelect()
    {
        OutfitConfigUI.instance.OpenOutfit(template.outfitConfig[outfitDropdown.value]);
        ManagerUI.instance.OpenPage(1);
    }

    public Custom_OutfitConfig GetCurrentOutfit()
    {
        return template.outfitConfig[outfitDropdown.value];
    }

    //----------------------------------------------------------------------------
    // Weapons
    //----------------------------------------------------------------------------
    public void AddWeapon(int type)
    {
        Transform content;
        switch (type)
        {
            default:
            case 0:
                content = primaryContent;
                break;
            case 1:
                content = secondaryContent;
                break;
            case 2:
                content = tertiaryContent;
                break;
        }

        GenericButton button = Global.SetupCollectionButton("", ItemType.Weapons, content);

        switch (type)
        {
            default:
            case 0:
                weaponOptionsID.Add(button);
                button.id = 0;
                break;
            case 1:
                weaponOptions_SecondaryID.Add(button);
                button.id = 1;
                break;
            case 2:
                weaponOptions_TertiaryID.Add(button);
                button.id = 2;
                break;
        }
    }
    public void RemoveWeapon(GenericButton button)
    {
        switch (button.id)
        {
            default:
            case 0:
                weaponOptionsID.Remove(button);
                break;
            case 1:
                weaponOptions_SecondaryID.Remove(button);
                break;
            case 2:
                weaponOptions_TertiaryID.Remove(button);
                break;
        }

        //Destroy Old Button
        Destroy(button.gameObject);
    }


    //----------------------------------------------------------------------------
    // Load Save
    //----------------------------------------------------------------------------

    /// <summary>
    /// Used for Load and New
    /// </summary>
    /// <param name="newTemplate"></param>
    public void Load(Custom_SosigEnemyTemplate newTemplate)
    {
        template = newTemplate;

        displayName.SetTextWithoutNotify(template.displayName);
        sosigEnemyCategory.SetTextWithoutNotify(template.sosigEnemyCategory.ToString());
        sosigEnemyID.SetTextWithoutNotify(template.sosigEnemyID.ToString());

        OutfitLoad();

        //Load Custom

        //Load Enemy Template
    }

    public void Save()
    {
        template.displayName = displayName.text;
        template.sosigEnemyCategory = int.Parse(sosigEnemyCategory.text);
        template.sosigEnemyID = int.Parse(sosigEnemyID.text);

        template.weaponOptionsID = Global.GenericButtonsToStringList(weaponOptionsID.ToArray());

        template.weaponOptions_SecondaryID = Global.GenericButtonsToStringList(weaponOptions_SecondaryID.ToArray());
        template.secondaryChance = int.Parse(secondaryChance.text);

        template.weaponOptions_TertiaryID = Global.GenericButtonsToStringList(weaponOptions_TertiaryID.ToArray());
        template.tertiaryChance = int.Parse(tertiaryChance.text);
    }

    //----------------------------------------------------------------------------
    // Sosig Enemy Category
    //----------------------------------------------------------------------------

    public void DropdownEnemyCategorySet()
    {
        //Just get the ID
        Debug.Log(sosigEnemyCategoryDropdown.options[sosigEnemyCategoryDropdown.value].text);
        string item = LibraryManager.GetInitialNumber(sosigEnemyCategoryDropdown.options[sosigEnemyCategoryDropdown.value].text);
        sosigEnemyCategory.SetTextWithoutNotify(item);
    }

    public void InputEnemyCategorySet()
    {
        for (int i = 0; i < sosigEnemyCategoryDropdown.options.Count; i++)
        {
            if (sosigEnemyCategoryDropdown.options[i].text.Contains(sosigEnemyCategory.text))
            {
                sosigEnemyCategoryDropdown.SetValueWithoutNotify(i);
                return;
            }
        }

        sosigEnemyCategoryDropdown.SetValueWithoutNotify(0);
    }

    void PopulateEnemyCategoryDropdown()
    {
        //TODO make this data external for future updates

        sosigEnemyCategoryDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        options.Add(new Dropdown.OptionData { text = "Custom" });

        foreach (SosigEnemyCategory category in Enum.GetValues(typeof(SosigEnemyCategory)))
        {
            string enumName = Enum.GetName(typeof(SosigEnemyCategory), category);
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = (int)category + "_" + enumName;
            options.Add(option);
        }
        sosigEnemyCategoryDropdown.AddOptions(options);
    }

    //----------------------------------------------------------------------------

}
