using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    public static ManagerUI instance;

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


    [Header("Pages")]
    public GameObject[] pages;


    [Header("Preview")]
    public Image previewSosig;
    public Image[] previewClothing;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        CloseAllPages();

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
        CloseAllPages();
        pages[i].SetActive(true);

        switch (i)
        {
            default:
                case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    void CloseAllPages()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
        }
    }

    //----------------------------------------------------------------------------
    // Preview
    //----------------------------------------------------------------------------


    public void GeneratePreview()
    {
        previewClothing[(int)WearType.Head].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
        previewClothing[(int)WearType.Eye].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Eye]);
        previewClothing[(int)WearType.Torso].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Torso]);
        previewClothing[(int)WearType.Pants].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Pants]);
        previewClothing[(int)WearType.PantsLower].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.PantsLower]);
        //previewClothing[(int)WearType.Backpacks].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
        //previewClothing[(int)WearType.Head].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
    }

    Sprite GetSosigClothing(OutfitConfigUI.Wear wear)
    {
        if (wear.buttons.Count >= 1)
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

        SosigEnemyTemplateUI.instance.Load(template);
        loadedSosig = true;
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
        SosigEnemyTemplateUI.instance.Load(new Custom_SosigEnemyTemplate());
        loadedSosig = true;
    }

    public void Save()
    {
        Debug.Log("Fake Saving!");
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

}
