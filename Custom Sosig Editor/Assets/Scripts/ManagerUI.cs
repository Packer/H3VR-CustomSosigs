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
    [SerializeField] List<Sprite> texturesBase = new List<Sprite>();
    public SosigIDCollection sosigEnemyIDs;

    public Sprite blankSprite;
    public Sprite defaultSprite;
    public static Sprite DefaultSprite() { return instance.defaultSprite; }

    public static List<Sprite> sosigs = new List<Sprite>();
    public static List<Sprite> accessories = new List<Sprite>();
    public static List<Sprite> weapons = new List<Sprite>();
    public static List<Sprite> textures = new List<Sprite>();

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

    [Header("Color Picker")]
    public GameObject colorPickerPanel;
    public Color prevColor;
    public Image newColor;
    public Image colorImage;
    public ColorPicker colorPicker;

    [Header("Preview")]
    public Camera previewCamera;
    public Image previewSosig;
    public SpriteRenderer[] previewClothing;
    public Image[] previewWeapons;
    public MeshRenderer[] sosigRenderers;
    public Transform sosigBody;
    public Material sosigMaterial;
    public Material sosigDefaultMaterial;
    public bool modfiyingSosigColor = false;

    private void Awake()
    {
        instance = this;
        versionText.text = "v" + Application.version;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupDefaultSosigMaterial();
        colorPicker.onColorChanged += OnColorChanged;

        CloseAllPages();
        saveButton.SetActive(false);
        mainMenu.SetActive(true);

        //Load our base items
        sosigs.AddRange(sosigsBase);
        accessories.AddRange(accessoriesBase);
        weapons.AddRange(weaponBase);
        textures.AddRange(texturesBase);

        //Setup SosigEnemyIds
        sosigEnemyIDs.sosigEnemyID = new List<int>();
        foreach (SosigEnemyID pieceType in System.Enum.GetValues(typeof(SosigEnemyID)))
        {
            sosigEnemyIDs.sosigEnemyID.Add((int)pieceType);
        }

        //Load Our Mods
        DataLoader.LoadCustomImages(0);
        DataLoader.LoadCustomImages(1);
        DataLoader.LoadCustomImages(2);
        DataLoader.LoadCustomImages(3);
    }
    private void OnDestroy()
    {
       colorPicker.onColorChanged -= OnColorChanged;
    }

    public void SetupDefaultSosigMaterial()
    {
        sosigMaterial = Instantiate(sosigDefaultMaterial);
        //Color event for sosigs
        for (int i = 0; i < sosigRenderers.Length; i++)
        {
            sosigRenderers[i].sharedMaterial = sosigMaterial;
        }
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


    public void OnColorChanged(Color c)
    {
        if (!modfiyingSosigColor)
            return;

        sosigMaterial.SetColor("_Color", c);
        sosigMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, c, 0.75f));
    }

    public void GeneratePreview()
    {
        //Clothing
        previewClothing[(int)WearType.Head].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Head]);
        previewClothing[(int)WearType.Eye].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Eye]);
        previewClothing[(int)WearType.Torso].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Torso]);
        previewClothing[(int)WearType.Pants].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Pants]);
        previewClothing[(int)WearType.PantsLower].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.PantsLower]);
        previewClothing[(int)WearType.Backpacks].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Backpacks]);
        previewClothing[(int)WearType.TorsoDecoration].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.TorsoDecoration]);
        previewClothing[(int)WearType.Belt].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Belt]);
        previewClothing[(int)WearType.Face].sprite = GetSosigClothing(OutfitConfigUI.instance.wears[(int)WearType.Face]);

        //Weapons
        previewWeapons[0].sprite = GetSosigWeapon(0);
        previewWeapons[1].sprite = GetSosigWeapon(1);
        previewWeapons[2].sprite = GetSosigWeapon(2);

        //Body Scale
        Custom_Sosig sosig = GetCustomSosig();
        Vector4 defaultPos = new Vector4(1.35f, 0.9f, 0.45f, 0);
        if (sosig != null)
        {
            //Set Scale
            sosigBody.localScale = sosig.scaleBody;
            sosigRenderers[3].transform.parent.localScale = sosig.scaleLegsLower;
            sosigRenderers[2].transform.parent.localScale = sosig.scaleLegsUpper;
            sosigRenderers[1].transform.parent.localScale = sosig.scaleTorso;
            sosigRenderers[0].transform.parent.localScale = sosig.scaleHead;

            //Hide
            sosigRenderers[0].enabled = !sosig.hideHeadMesh;
            sosigRenderers[1].enabled = !sosig.hideTorsoMesh;
            sosigRenderers[2].enabled = !sosig.hideLegsUpperMesh;
            sosigRenderers[3].enabled = !sosig.hideLegsLowerMesh;

            //Offset Positions
            sosigRenderers[3].transform.parent.localPosition = new Vector3(0, 0, 0);
            Vector3 height = new Vector3(0, 0.225f * sosig.scaleLegsLower.y, 0);

            height += new Vector3(0, 0.225f * sosig.scaleLegsUpper.y, 0);
            sosigRenderers[2].transform.parent.localPosition = height;
            height += new Vector3(0, 0.225f * sosig.scaleLegsUpper.y, 0);

            height += new Vector3(0, 0.225f * sosig.scaleTorso.y, 0);
            sosigRenderers[1].transform.parent.localPosition = height;
            height += new Vector3(0, 0.225f * sosig.scaleTorso.y, 0);

            height += new Vector3(0, 0.225f * sosig.scaleHead.y, 0);
            sosigRenderers[0].transform.parent.localPosition = height;
        }
    }

    public void TakeScreenshot()
    {
        DataLoader.TakeScreenshot(
            SosigEnemyTemplateUI.instance.template.SosigEnemyID.ToString() + "_" + SosigEnemyTemplateUI.instance.template.DisplayName,
            previewCamera);
    }

    Custom_Sosig GetCustomSosig()
    {
        return SosigEnemyTemplateUI.instance.GetCurrentCustomSosig();
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
        template.CustomSosigs.Add(new Custom_Sosig());
        template.OutfitConfigs.Add(new Custom_OutfitConfig());
        template.Configs.Add(new Custom_SosigConfigTemplate());

        SosigEnemyTemplateUI.instance.LoadEnemyTemplate(template);
        loadedSosig = true;
        saveButton.SetActive(true);
        mainMenu.SetActive(false);

        //Reset Preview
        SosigEnemyTemplateUI.instance.sosigEnemyCategoryDropdown.SetValueWithoutNotify(1);

        CustomSosigUI.instance.useCustomSkin.SetValueWithoutNotify(0);
        SetupDefaultSosigMaterial();
        CustomSosigUI.instance.UpdateSosigSkinDropdown();
        CustomSosigUI.instance.UpdateSosigTexture();
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

        if (SosigEnemyTemplateUI.instance.template.CustomSosigs.Count > 0)
        {
            switch (SosigEnemyTemplateUI.instance.template.CustomSosigs[0].customSkin)
            {
                case "":
                    CustomSosigUI.instance.useCustomSkin.value = 0;
                    break;
                case "CustomSosig_Base":
                    CustomSosigUI.instance.useCustomSkin.value = 1;
                    break;
                default:    //Any text
                    CustomSosigUI.instance.useCustomSkin.value = 2;
                    break;
            }
        }

        CustomSosigUI.instance.UpdateSosigSkinDropdown();
        CustomSosigUI.instance.UpdateSosigTexture();
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
    // Color Picker
    //----------------------------------------------------------------------------

    public void ConfirmColor()
    {
        if (colorImage)
            colorImage.color = newColor.color;
        colorPickerPanel.SetActive(false);
        colorImage = null;

        if (modfiyingSosigColor)
            CustomSosigUI.instance.SaveCustomSosig();

        modfiyingSosigColor = false;
    }

    public void OpenSosigColor(Image previewImage)
    {
        modfiyingSosigColor = true;
        OpenColorPicker(previewImage);
    }

    public void OpenColorPicker(Image previewImage)
    {
        colorImage = previewImage;
        prevColor = previewImage.color;
        colorPickerPanel.SetActive(true);
        colorPicker.color = previewImage.color;
    }

    public void CancelColor()
    {
        if (colorImage)
        {
            colorImage.color = prevColor;
        }
        colorPickerPanel.SetActive(false);
        colorImage = null;

        if (modfiyingSosigColor)
            OnColorChanged(prevColor);

        modfiyingSosigColor = false;
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
