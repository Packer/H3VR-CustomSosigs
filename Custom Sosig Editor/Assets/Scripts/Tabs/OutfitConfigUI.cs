using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutfitConfigUI : MonoBehaviour
{
    public static OutfitConfigUI instance;

    public Custom_OutfitConfig outfitConfig;
    public GameObject collectionPrefab;

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

    public class Wear
    {
        public WearType type = WearType.Head;
        public List<GenericButton> buttons;
        public Transform content;
        public InputField[] IDs;
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

    public Wear[] wears = new Wear[8];

    [Header("Attributes")]
    private List<GenericButton> headwearButtons;
    [SerializeField] Transform headwearContent;
    public InputField[] headwearID;
    public InputField chance_HeadWear;
    public Toggle headUsesTorsoIndex;

    private List<GenericButton> eyewearButtons;
    [SerializeField] Transform eyewearContent;
    public InputField[] eyewearID;
    public InputField chance_Eyewear;

    private List<GenericButton> torsowearButtons;
    [SerializeField] Transform torsowearContent;
    public InputField[] torsowearID;
    public InputField chance_Torsowear;

    private List<GenericButton> pantswearButtons;
    [SerializeField] Transform pantswearContent;
    public InputField[] pantswearID;
    public InputField chance_Pantswear;
    public Toggle pantsUsesTorsoIndex;

    private List<GenericButton> pantswearLowerButtons;
    [SerializeField] Transform pantswearLowerContent;
    public InputField[] pantswear_LowerID;
    public InputField chance_Pantswear_Lower;
    public Toggle pantsLowerUsesPantsIndex;

    private List<GenericButton> backpacksButtons;
    [SerializeField] Transform backpacksContent;
    public InputField[] backpacksID;
    public InputField chance_Backpacks;

    private List<GenericButton> torsoDecorationButtons;
    [SerializeField] Transform torsoDecorationContent;
    public InputField[] torsoDecorationID;
    public InputField chance_TorsoDecoration;

    private List<GenericButton> beltButtons;
    [SerializeField] Transform beltContent;
    public InputField[] beltID;
    public InputField chance_belt;


    public void Awake()
    {
        instance = this;
    }


    public void OpenOutfit(Custom_OutfitConfig outfit)
    {

        outfitConfig = outfit;

        headwearButtons = Global.SetupCollection(outfit.headwearID, ItemType.Accessories, headwearContent);

    }

    public void SaveOutfit()
    {
        
    }

}