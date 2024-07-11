using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI instance;
    public Text versionText;

    [Header("Debug")]
    [SerializeField] Text debugLine;
    private string debugLog = "";

    [SerializeField] List<Sprite> sosigsBase = new List<Sprite>();
    [SerializeField] List<Sprite> accessoriesBase = new List<Sprite>();
    [SerializeField] List<Sprite> weaponBase = new List<Sprite>();

    public Sprite blankSprite;
    public Sprite defaultSprite;
    public static Sprite DefaultSprite() { return instance.defaultSprite; }

    public static List<Sprite> sosigs = new List<Sprite>();
    public static List<Sprite> accessories = new List<Sprite>();
    public static List<Sprite> weapons = new List<Sprite>();

    private string sosigEnemyIDsPath = "";
    private string accessoriesPath = "";
    private string weaponIDsPath = "";

    public GameObject sosigCollectionPrefab;
    public GameObject weaponsCollectionPrefab;
    public GameObject accessoriesCollectionPrefab;

    [Header("Popup Warning")]
    public GameObject warningPanel;
    public Text warningMessage;
    public string warningConfirmMethod = "";
    private bool loadedSosig = false;
    public GameObject loadingScreen;


    [Header("Pages")]
    public GameObject[] pages;
    public GameObject mainMenu;
    public GameObject saveButton;


    [Header("Preview")]
    public Image previewSosig;
    public Image[] previewClothing;
    public Image[] previewWeapons;


    private void Awake()
    {
        instance = this;
        versionText.text = "v" + Application.version;
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseAllPages();
        saveButton.SetActive(false);
        mainMenu.SetActive(true);

        //Load our base items
        sosigs.AddRange(sosigsBase);
        accessories.AddRange(accessoriesBase);
        weapons.AddRange(weaponBase);

        return; //For now

        //Setup Path (Incase its moved)
        sosigEnemyIDsPath = Application.dataPath + "\\Sosigs\\";
        accessoriesPath = Application.dataPath + "\\Accessories\\";
        weaponIDsPath = Application.dataPath + "\\Weapons\\";


        //Load our Mods
        sosigs.AddRange(DataLoader.LoadExternalMods(DataLoader.GetSubDirectories(sosigEnemyIDsPath)));
        accessories.AddRange(DataLoader.LoadExternalMods(DataLoader.GetSubDirectories(accessoriesPath)));
        weapons.AddRange(DataLoader.LoadExternalMods(DataLoader.GetSubDirectories(weaponIDsPath)));
    }

    //----------------------------------------------------------------------------
    // Page Management
    //----------------------------------------------------------------------------

    public void OpenPage(int i)
    {
        CloseAllPages(i);
    }

    void CloseAllPages(int index = -1)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (index == i)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
    }

    //----------------------------------------------------------------------------
    // Preview
    //----------------------------------------------------------------------------


    public void GeneratePreview()
    {
        //Clothing
        previewClothing[(int)WearType.Head].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
        previewClothing[(int)WearType.Eye].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Eye]);
        previewClothing[(int)WearType.Torso].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Torso]);
        previewClothing[(int)WearType.Pants].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Pants]);
        previewClothing[(int)WearType.PantsLower].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.PantsLower]);
        //previewClothing[(int)WearType.Backpacks].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
        //previewClothing[(int)WearType.Head].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);

        //Weapons
        previewWeapons[0].sprite = GetSosigWeapon(0);
        previewWeapons[1].sprite = GetSosigWeapon(1);
        previewWeapons[2].sprite = GetSosigWeapon(2);
    }

    Sprite GetSosigWeapon(int index)
    {
        bool success = index == 0 ? true : false;

        List<GenericButton> buttons;
        if (index == 1)
        {
            buttons = SosigEnemyTemplateUI.instance.weaponOptions_SecondaryID;
            success = Random.Range(0.00f, 1.00f) <= float.Parse(SosigEnemyTemplateUI.instance.secondaryChance.text);
        }
        else if (index == 2)
        {
            buttons = SosigEnemyTemplateUI.instance.weaponOptions_TertiaryID;
            success = Random.Range(0.00f, 1.00f) <= float.Parse(SosigEnemyTemplateUI.instance.tertiaryChance.text);
        }
        else
        {
            buttons = SosigEnemyTemplateUI.instance.weaponOptionsID;
            success = true;
        }

        if (buttons.Count >= 1 && success)
            return buttons[Random.Range(0, buttons.Count)].image.sprite;
        else
            return blankSprite;
    }

    Sprite GetSosigClothing(OutfitConfigUI.Wear wear)
    {
        if (wear.buttons.Count >= 1 && Random.Range(0.00f, 1.00f) <= float.Parse(wear.chance.text))
            return wear.buttons[Random.Range(0, wear.buttons.Count)].image.sprite;
        else
            return blankSprite;
    }


    //----------------------------------------------------------------------------
    // New, Load, Save
    //----------------------------------------------------------------------------

    public void New()
    {
        if (loadedSosig)
            PopupWarning(nameof(NewSosig), "A new sosig will overwrite the previous one, are you sure?");
        else
            NewSosig();
    }

    void NewSosig()
    {
        Custom_SosigEnemyTemplate template = new Custom_SosigEnemyTemplate();
        template.customSosig.Add(new Custom_Sosig());
        template.outfitConfig.Add(new Custom_OutfitConfig());
        template.configTemplates.Add(new Custom_SosigConfigTemplate());

        SosigEnemyTemplateUI.instance.LoadEnemyTemplate(template);
        loadedSosig = true;
        saveButton.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Load()
    {
        if (loadedSosig)
            PopupWarning(nameof(LoadSosig), "Loading a sosig will overwrite the previous one, are you sure?");
        else
            LoadSosig();
    }

    void LoadSosig()
    {
        //GET EXTERNAL TEMPLATE HERE
        if (!DataLoader.OnLoadDialogue())
            return;
        //SosigEnemyTemplateUI.instance.LoadEnemyTemplate(new Custom_SosigEnemyTemplate());
        loadedSosig = true;
        saveButton.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Save()
    {
        //Last save just in case
        SosigEnemyTemplateUI.instance.SaveEnemyTemplate();

        string json = JsonUtility.ToJson(SosigEnemyTemplateUI.instance.template, true);
        DataLoader.OnSaveDialogue(json);
    }

    //----------------------------------------------------------------------------
    //Popup Warning
    //----------------------------------------------------------------------------

    public void PopupWarning(string method, string text)
    {
        warningConfirmMethod = method;
        warningMessage.text = text;
        warningPanel.SetActive(true);
    }

    public void ConfirmWarning()
    {
        warningPanel.SetActive(false);
        if (warningConfirmMethod == "")
            return;

        Invoke(warningConfirmMethod, 0);
        warningConfirmMethod = "";
    }

    public void CloseWarning()
    {
        warningPanel.SetActive(false);
        warningConfirmMethod = "";
    }

    //----------------------------------------------------------------------------
    // Console Log
    //----------------------------------------------------------------------------

    public static void LogError(string text)
    {
        instance.debugLog += "\n" + text;
        instance.debugLine.text = text;
        instance.debugLine.color = Color.red;
        Debug.LogError(text);
    }

    public static void Log(string text)
    {
        instance.debugLog += "\n" + text;
        instance.debugLine.text = text;
        instance.debugLine.color = Color.white;
        Debug.Log(text);
    }
}
