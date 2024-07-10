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


    public static List<Sprite> sosigs = new List<Sprite>();
    public static List<Sprite> accessories = new List<Sprite>();
    public static List<Sprite> weapons = new List<Sprite>();

    private string sosigEnemyIDsPath = "";
    private string accessoriesPath = "";
    private string weaponIDsPath = "";

    public GameObject sosigCollectionPrefab;
    public GameObject weaponsCollectionPrefab;
    public GameObject accessoriesCollectionPrefab;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }

}
