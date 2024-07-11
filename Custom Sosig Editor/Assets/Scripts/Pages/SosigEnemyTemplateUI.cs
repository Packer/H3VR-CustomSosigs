using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SosigEnemyTemplateUI : MonoBehaviour
{
    public static SosigEnemyTemplateUI instance;

    public Custom_SosigEnemyTemplate template;
    public Text outfitTitleText;

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

    [Header("Config Dropdowns")]
    public Dropdown outfitDropdown;
    public Dropdown customSosigDropdown;
    public Dropdown configTemplateDropdown;

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
        int lastValue = outfitDropdown.value;
        outfitDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < template.outfitConfig.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.outfitConfig[i].name;
            options.Add(option);
        }
        outfitDropdown.AddOptions(options);

        if(lastValue < outfitDropdown.options.Count)
            outfitDropdown.value = lastValue;
        else
            outfitDropdown.value = 0;

        OutfitSelect();
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
    }

    public void OutfitAdd()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_OutfitConfig outfit = new Custom_OutfitConfig();
        option.text = outfit.name;
        template.outfitConfig.Add(outfit);
        outfitDropdown.options.Add(option);
        outfitDropdown.value++;
        OutfitSelect();
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
    }

    public void OutfitDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_OutfitConfig outfit = Global.ObjectCloner.Clone(template.outfitConfig[outfitDropdown.value]);
        option.text = outfit.name;
        template.outfitConfig.Add(outfit);
        outfitDropdown.options.Add(option);
        outfitDropdown.value++;
        OutfitSelect();
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
    }

    public void OutfitRemove()
    {
        //Don't delete last option
        if (outfitDropdown.options.Count <= 1)
            return;

        template.outfitConfig.RemoveAt(outfitDropdown.value);
        outfitDropdown.options.RemoveAt(outfitDropdown.value);
        outfitDropdown.value--;
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
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
    // Config Template Menu
    //----------------------------------------------------------------------------

    public void ConfigTemplateLoad()
    {
        int lastValue = configTemplateDropdown.value;
        configTemplateDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < template.configTemplates.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.configTemplates[i].name;
            options.Add(option);
        }
        configTemplateDropdown.AddOptions(options);

        if (lastValue < configTemplateDropdown.options.Count)
            configTemplateDropdown.value = lastValue;
        else
            configTemplateDropdown.value = 0;

        ConfigTemplateSelect();
        outfitTitleText.text = "CONFIG TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateAdd()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_SosigConfigTemplate outfit = new Custom_SosigConfigTemplate();
        option.text = outfit.name;
        template.configTemplates.Add(outfit);
        configTemplateDropdown.options.Add(option);
        configTemplateDropdown.value++;
        ConfigTemplateSelect();
        outfitTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_SosigConfigTemplate config = template.configTemplates[configTemplateDropdown.value].Clone();
        option.text = config.name;
        template.configTemplates.Add(config);
        configTemplateDropdown.options.Add(option);
        configTemplateDropdown.value++;
        ConfigTemplateSelect();
        outfitTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateRemove()
    {
        //Don't delete last option
        if (configTemplateDropdown.options.Count <= 1)
            return;

        template.configTemplates.RemoveAt(configTemplateDropdown.value);
        configTemplateDropdown.options.RemoveAt(configTemplateDropdown.value);
        configTemplateDropdown.value--;
        outfitTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateSelect()
    {
        ConfigTemplateUI.instance.OpenConfigTemplate(template.configTemplates[configTemplateDropdown.value]);
        ManagerUI.instance.OpenPage(2);
    }

    public Custom_SosigConfigTemplate GetCurrentConfig()
    {
        return template.configTemplates[configTemplateDropdown.value];
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

        SaveEnemyTemplate();
    }

    public void ClearAllWeapons()
    {
        for (int i = 0; i < weaponOptionsID.Count; i++)
        {
            if(weaponOptionsID[i])
                Destroy(weaponOptionsID[i].gameObject);
        }
        weaponOptionsID.Clear();

        for (int i = 0; i < weaponOptions_SecondaryID.Count; i++)
        {
            if (weaponOptions_SecondaryID[i])
                Destroy(weaponOptions_SecondaryID[i].gameObject);
        }
        weaponOptions_SecondaryID.Clear();

        for (int i = 0; i < weaponOptions_TertiaryID.Count; i++)
        {
            if (weaponOptions_TertiaryID[i])
                Destroy(weaponOptions_TertiaryID[i].gameObject);
        }
        weaponOptions_TertiaryID.Clear();
    }

    //----------------------------------------------------------------------------
    // Load Save
    //----------------------------------------------------------------------------

    /// <summary>
    /// Used for Load and New
    /// </summary>
    /// <param name="newTemplate"></param>
    public void LoadEnemyTemplate(Custom_SosigEnemyTemplate newTemplate)
    {
        template = newTemplate;

        displayName.SetTextWithoutNotify(template.displayName);
        sosigEnemyCategory.SetTextWithoutNotify(template.sosigEnemyCategory.ToString());
        sosigEnemyID.SetTextWithoutNotify(template.sosigEnemyID.ToString());

        //Weapons
        ClearAllWeapons();

        weaponOptionsID = Global.SetupCollection(template.weaponOptionsID, ItemType.Weapons, primaryContent);
        weaponOptions_SecondaryID = Global.SetupCollection(template.weaponOptions_SecondaryID, ItemType.Weapons, secondaryContent);
        weaponOptions_TertiaryID = Global.SetupCollection(template.weaponOptions_TertiaryID, ItemType.Weapons, tertiaryContent);

        secondaryChance.text = template.secondaryChance.ToString();
        tertiaryChance.text = template.tertiaryChance.ToString();

        //Load Outfits
        OutfitLoad();

        //Load Custom

        //Load Enemy Template
        ConfigTemplateLoad();

        ManagerUI.instance.GeneratePreview();
    }

    public void SaveEnemyTemplate()
    {
        template.displayName = displayName.text;
        if (sosigEnemyCategory.text == "")
            template.sosigEnemyCategory = 0;
        else
            template.sosigEnemyCategory = int.Parse(sosigEnemyCategory.text);
        template.sosigEnemyID = int.Parse(sosigEnemyID.text);

        template.weaponOptionsID = Global.GenericButtonsToStringList(weaponOptionsID.ToArray());

        template.weaponOptions_SecondaryID = Global.GenericButtonsToStringList(weaponOptions_SecondaryID.ToArray());
        template.secondaryChance = float.Parse(secondaryChance.text);

        template.weaponOptions_TertiaryID = Global.GenericButtonsToStringList(weaponOptions_TertiaryID.ToArray());
        template.tertiaryChance = float.Parse(tertiaryChance.text);

        ManagerUI.Log("Enemy Template applied at: " + System.DateTime.Now);
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
