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
    public Text configTemplateTitleText;
    public Text customSosigTitleText;

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
        for (int i = 0; i < template.OutfitConfigs.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.OutfitConfigs[i].name;
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
        template.OutfitConfigs.Add(outfit);
        outfitDropdown.options.Add(option);
        outfitDropdown.value++;
        OutfitSelect();
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
    }

    public void OutfitDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_OutfitConfig outfit = Global.ObjectCloner.Clone(template.OutfitConfigs[outfitDropdown.value]);
        option.text = outfit.name;
        template.OutfitConfigs.Add(outfit);
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

        template.OutfitConfigs.RemoveAt(outfitDropdown.value);
        outfitDropdown.options.RemoveAt(outfitDropdown.value);
        outfitDropdown.value--;
        outfitTitleText.text = "OUTFIT CONFIG: " + outfitDropdown.options[outfitDropdown.value].text;
    }

    public void OutfitSelect()
    {
        OutfitConfigUI.instance.OpenOutfit(template.OutfitConfigs[outfitDropdown.value]);
        ManagerUI.instance.OpenPage(1);
    }

    public Custom_OutfitConfig GetCurrentOutfit()
    {
        return template.OutfitConfigs[outfitDropdown.value];
    }

    //----------------------------------------------------------------------------
    // CustomSosig Config Menu
    //----------------------------------------------------------------------------

    public void CustomSosigLoad()
    {
        int lastValue = customSosigDropdown.value;
        customSosigDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < template.CustomSosigs.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.CustomSosigs[i].name;
            options.Add(option);
        }
        customSosigDropdown.AddOptions(options);

        if (lastValue < customSosigDropdown.options.Count)
            customSosigDropdown.value = lastValue;
        else
            customSosigDropdown.value = 0;

        CustomSosigSelect();
        customSosigTitleText.text = "CUSTOM SOSIG: " + customSosigDropdown.options[customSosigDropdown.value].text;
    }

    public void CustomSosigAdd()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_Sosig sosig = new Custom_Sosig();
        option.text = sosig.name;
        template.CustomSosigs.Add(sosig);
        customSosigDropdown.options.Add(option);
        customSosigDropdown.value++;
        CustomSosigSelect();
        customSosigTitleText.text = "CUSTOM SOSIG: " + customSosigDropdown.options[customSosigDropdown.value].text;
    }

    public void CustomSosigDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_Sosig sosig = template.CustomSosigs[customSosigDropdown.value].Clone();
        option.text = sosig.name;
        template.CustomSosigs.Add(sosig);
        customSosigDropdown.options.Add(option);
        customSosigDropdown.value++;
        CustomSosigSelect();
        customSosigTitleText.text = "CUSTOM SOSIG: " + customSosigDropdown.options[customSosigDropdown.value].text;
    }

    public void CustomSosigRemove()
    {
        //Don't delete last option
        if (customSosigDropdown.options.Count <= 1)
            return;

        template.CustomSosigs.RemoveAt(customSosigDropdown.value);
        customSosigDropdown.options.RemoveAt(customSosigDropdown.value);
        customSosigDropdown.value--;
        customSosigTitleText.text = "CUSTOM SOSIG: " + customSosigDropdown.options[customSosigDropdown.value].text;
    }

    public void CustomSosigSelect()
    {
        CustomSosigUI.instance.OpenCustomSosig(template.CustomSosigs[customSosigDropdown.value]);
        ManagerUI.instance.OpenPage(3);
    }

    public Custom_Sosig GetCurrentCustomSosig()
    {
        return template.CustomSosigs[customSosigDropdown.value];
    }

    //----------------------------------------------------------------------------
    // Config Template Menu
    //----------------------------------------------------------------------------

    public void ConfigTemplateLoad()
    {
        int lastValue = configTemplateDropdown.value;
        configTemplateDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < template.Configs.Count; i++)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = template.Configs[i].name;
            options.Add(option);
        }
        configTemplateDropdown.AddOptions(options);

        if (lastValue < configTemplateDropdown.options.Count)
            configTemplateDropdown.value = lastValue;
        else
            configTemplateDropdown.value = 0;

        ConfigTemplateSelect();
        configTemplateTitleText.text = "CONFIG TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateAdd()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_SosigConfigTemplate outfit = new Custom_SosigConfigTemplate();
        option.text = outfit.name;
        template.Configs.Add(outfit);
        configTemplateDropdown.options.Add(option);
        configTemplateDropdown.value++;
        ConfigTemplateSelect();
        configTemplateTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateDuplicate()
    {
        Dropdown.OptionData option = new Dropdown.OptionData();
        Custom_SosigConfigTemplate config = template.Configs[configTemplateDropdown.value].Clone();
        option.text = config.name;
        template.Configs.Add(config);
        configTemplateDropdown.options.Add(option);
        configTemplateDropdown.value++;
        ConfigTemplateSelect();
        configTemplateTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateRemove()
    {
        //Don't delete last option
        if (configTemplateDropdown.options.Count <= 1)
            return;

        template.Configs.RemoveAt(configTemplateDropdown.value);
        configTemplateDropdown.options.RemoveAt(configTemplateDropdown.value);
        configTemplateDropdown.value--;
        configTemplateTitleText.text = "OUTFIT TEMPLATE: " + configTemplateDropdown.options[configTemplateDropdown.value].text;
    }

    public void ConfigTemplateSelect()
    {
        ConfigTemplateUI.instance.OpenConfigTemplate(template.Configs[configTemplateDropdown.value]);
        ManagerUI.instance.OpenPage(2);
    }

    public Custom_SosigConfigTemplate GetCurrentConfig()
    {
        return template.Configs[configTemplateDropdown.value];
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

        displayName.SetTextWithoutNotify(template.DisplayName);
        sosigEnemyCategory.SetTextWithoutNotify(template.SosigEnemyCategory.ToString());
        sosigEnemyID.SetTextWithoutNotify(template.SosigEnemyID.ToString());

        //Weapons
        ClearAllWeapons();

        weaponOptionsID = Global.SetupCollection(template.WeaponOptions, ItemType.Weapons, primaryContent);
        weaponOptions_SecondaryID = Global.SetupCollection(template.WeaponOptionsSecondary, ItemType.Weapons, secondaryContent);
        weaponOptions_TertiaryID = Global.SetupCollection(template.WeaponOptionsTertiary, ItemType.Weapons, tertiaryContent);

        secondaryChance.text = template.SecondaryChance.ToString();
        tertiaryChance.text = template.TertiaryChance.ToString();

        //Load Outfits
        OutfitLoad();

        //Load Custom
        CustomSosigLoad();

        //Load Enemy Template
        ConfigTemplateLoad();

        ManagerUI.instance.GeneratePreview();
    }

    public void SaveEnemyTemplate()
    {
        template.DisplayName = displayName.text;
        if (sosigEnemyCategory.text == "")
            template.SosigEnemyCategory = 0;
        else
            template.SosigEnemyCategory = int.Parse(sosigEnemyCategory.text);
        template.SosigEnemyID = int.Parse(sosigEnemyID.text);

        template.WeaponOptions = Global.GenericButtonsToStringList(weaponOptionsID.ToArray());

        template.WeaponOptionsSecondary = Global.GenericButtonsToStringList(weaponOptions_SecondaryID.ToArray());
        template.SecondaryChance = float.Parse(secondaryChance.text);

        template.WeaponOptionsTertiary = Global.GenericButtonsToStringList(weaponOptions_TertiaryID.ToArray());
        template.TertiaryChance = float.Parse(tertiaryChance.text);

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

    public void SosigEnemyIDCheck()
    {
        int sosigID = int.Parse(sosigEnemyID.text);

        //ColorBlock block = sosigEnemyID.colors;
        
        if (ManagerUI.instance.sosigEnemyIDs.sosigEnemyID.Contains(sosigID))
            sosigEnemyID.textComponent.color = Color.red;
        else
            sosigEnemyID.textComponent.color = Color.white;
        //sosigEnemyID.colors = block;
    }

    //----------------------------------------------------------------------------

}
