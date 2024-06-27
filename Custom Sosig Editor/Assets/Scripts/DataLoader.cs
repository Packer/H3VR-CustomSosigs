using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class DataLoader : MonoBehaviour
{
    public static List<Sprite> loadedSprites = new List<Sprite>();

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
