using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SosigEnemyTemplateUI : MonoBehaviour
{
    public static SosigEnemyTemplateUI instance;

    public Custom_SosigEnemyTemplate template;

    [Header("Attributes")]
    public InputField displayName;
    public InputField sosigEnemyCategory;
    public Dropdown sosigEnemyCategoryDropdown;
    public InputField sosigEnemyID;

    public InputField[] weaponOptionsID;
    public InputField[] weaponOptions_SecondaryID;
    public InputField secondaryChance;
    public InputField[] weaponOptions_TertiaryID;
    public InputField tertiaryChance;

    public Custom_Sosig[] customSosig;
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

    public void Load()
    {
        displayName.SetTextWithoutNotify(template.displayName);
        sosigEnemyCategory.SetTextWithoutNotify(template.sosigEnemyCategory.ToString());
        sosigEnemyID.SetTextWithoutNotify(template.sosigEnemyID.ToString());
    }

    public void Save()
    {
        template.displayName = displayName.text;
        template.sosigEnemyCategory = int.Parse(sosigEnemyCategory.text);
        template.sosigEnemyID = int.Parse(sosigEnemyID.text);
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
