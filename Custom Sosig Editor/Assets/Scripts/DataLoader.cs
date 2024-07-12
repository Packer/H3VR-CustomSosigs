using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using SFB;
using System;

public class DataLoader : MonoBehaviour
{
    public static List<Sprite> loadedSprites = new List<Sprite>();
    public static Custom_SosigEnemyTemplate sosigEnemyTemplate;
    public static string lastDirectory;

    public static void OnSaveDialogue(string json)
    {
        string path = StandaloneFileBrowser.SaveFilePanel(
            "Save Custom Sosig",
            Path.GetDirectoryName(lastDirectory),
            Path.GetFileName(lastDirectory),
            "csosig");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, json);
            ManagerUI.Log("Saved Custom Sosig " + path);
        }
    }

    public static bool OnLoadDialogue()
    {
        string[] paths;
        string modPath = lastDirectory;

        if (modPath == "")
            modPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/r2modmanPlus-local/H3VR/profiles/";

        if (!Directory.Exists(modPath))
            modPath = "";

        paths = StandaloneFileBrowser.OpenFilePanel("Load Custom Sosig", modPath, "csosig", false);


        if (paths.Length > 0)
        {
            lastDirectory = paths[0];
            PlayerPrefs.SetString("lastCharacterDirectory", lastDirectory);

            ManagerUI.instance.StartCoroutine(OutputRoutine(new Uri(paths[0]).AbsoluteUri));
        }
        else
        {
            ManagerUI.Log("Loading Custom Sosig Canceled");
            return false;
        }

        return true;
    }

    public static IEnumerator OutputRoutine(string url)
    {
        var loader = new WWW(url);
        yield return loader;

        Custom_SosigEnemyTemplate template = LoadCustomSosig(loader.text);
        yield return null;

        if (template != null)
            SosigEnemyTemplateUI.instance.LoadEnemyTemplate(template);

    }

    public static Custom_SosigEnemyTemplate LoadCustomSosig(string json)
    {
        try
        {
            sosigEnemyTemplate = JsonUtility.FromJson<Custom_SosigEnemyTemplate>(json);
        }
        catch (Exception ex)
        {
            ManagerUI.LogError(ex.Message);
            return null;
        }

        if (sosigEnemyTemplate != null)
        {
            ManagerUI.Log("Loaded Custom Sosig " + sosigEnemyTemplate.displayName);
        }

        return sosigEnemyTemplate;
    }


    public static Sprite LoadSprite(string path)
    {
        //Debug.Log("Supply Raid - Loading: " + path);
        Texture2D tex = null;

        byte[] fileData;

        if (File.Exists(path) && tex == null)
        {
            fileData = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }

        if (tex == null)
        {
            Debug.LogError("Texture Not Found: " + path);
            return null;
        }
        Sprite NewSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0), 100.0f);
        NewSprite.name = Path.GetFileName(path).Replace(".png", "");
        loadedSprites.Add(NewSprite);

        return NewSprite;
    }

    public static List<string> GetSubDirectories(string location)
    {
        return Directory.GetFiles(location, "*.png", SearchOption.AllDirectories).ToList();
    }

    public static List<Sprite> LoadExternalMods(List<string> directories)
    {
        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < directories.Count; i++)
        {
            sprites.Add(LoadSprite(directories[i]));
        }

        return sprites;
    }
    public static void LoadCustomImages(int index)
    {

        string path;
        switch (index)
        {
            default:
            case 0: //Sosigs
                path = "/../" + "Sosigs";
                break;
            case 1: //Weapons
                path = "/../" + "Weapons";
                break;
            case 2: //Accessories ItemType.Accessories
                path = "/../" + "Accessories";
                break;
        }

        string modFolder = Application.dataPath + path; //"/../" + "CustomSosigs";

        modFolder = Directory.CreateDirectory(modFolder).FullName;
        //ManagerUI.Log("Mod Folder is at: " + modFolder);

        List<string> directories = Directory.GetFiles(modFolder, "*.png", SearchOption.AllDirectories).ToList();

        //Debug.Log("Count: " + directories.Count);
        if (directories.Count == 0)
            return;

        //Load up each of our categories
        for (int i = 0; i < directories.Count; i++)
        {
            Sprite newSprite = LoadSprite(directories[i]);

            //Add custom sosigs to our list of used sosigenemyIDS
            if (index == (int)ItemType.Sosigs)
            {
                int newID;
                if (int.TryParse(Global.GetInitialNumber(newSprite.name), out newID))
                {
                    if (!ManagerUI.instance.sosigEnemyIDs.sosigEnemyID.Contains(newID))
                        ManagerUI.instance.sosigEnemyIDs.sosigEnemyID.Add(newID);
                }
                if (!ManagerUI.sosigs.Contains(newSprite))
                    ManagerUI.sosigs.Add(newSprite);
            }
            else if (index == (int)ItemType.Weapons)
            {
                if (!ManagerUI.weapons.Contains(newSprite))
                    ManagerUI.weapons.Add(newSprite);
            }
            else if (index == (int)ItemType.Accessories)
            {
                if (!ManagerUI.accessories.Contains(newSprite))
                    ManagerUI.accessories.Add(newSprite);
            }

        }
    }

    /*

    public static List<string> GetDirectories(string location)
    {
        List<string> directories = Directory.GetFiles(location, "*.png", SearchOption.AllDirectories).ToList();

        if (directories.Count == 0)
        {
            Debug.LogError("No Factions were found!");
            return null;
        }

        List<SR_SosigFaction> factions = new List<SR_SosigFaction>();

        //Load up each of our categories
        for (int i = 0; i < directories.Count; i++)
        {
            SR_SosigFaction faction;

            //Load each Category via the Directory
            using (StreamReader streamReader = new StreamReader(directories[i]))
            {
                string json = streamReader.ReadToEnd();

                try
                {
                    faction = JsonUtility.FromJson<SR_SosigFaction>(json);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.Message);
                    return null;
                }

                //Add to our item category pool
                factions.Add(faction);
                string newDirectory = directories[i];
                newDirectory = newDirectory.Remove(newDirectory.Length - 4) + "png";
                faction.SetupThumbnailPath(newDirectory);

                Debug.Log("Supply Raid: Loaded Faction " + faction.name);
            }
        }
        return factions;
    }
    */
}
