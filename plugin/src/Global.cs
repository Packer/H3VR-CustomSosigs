using System.Collections.Generic;
using FistVR;
using UnityEngine;
using BepInEx;
using System.IO;
using System.Linq;

namespace CustomSosigLoader
{
    internal class Global
    {
        //public static List<Texture2D> loadedTextures = new List<Texture2D>();
        public static Texture2D whiteSosig;
        public static List<SosigMaterial> sosigMaterials;

        public class SosigMaterial
        {
            public string name;
            public Texture2D albedo;
            public Texture2D normal;
            public Texture masr;
        }


        public static List<string> GetCustomSosigDirectories()
        {
            return Directory.GetFiles(Paths.PluginPath, "*.csosig", SearchOption.AllDirectories).ToList();
        }


        public static void LoadCustomSosigs()
        {
            List<string> directories = GetCustomSosigDirectories();

            if (directories.Count == 0)
            {
                Debug.Log("Custom Sosig Loader - No Custom Sosigs were found!");
                return;
            }

            //List<Custom_SosigEnemyTemplate> items = new List<Custom_SosigEnemyTemplate>();

            //Load up each of our categories
            for (int i = 0; i < directories.Count; i++)
            {
                Custom_SosigEnemyTemplate sosigTemplate;

                //Load each Category via the Directory
                using (StreamReader streamReader = new StreamReader(directories[i]))
                {
                    string json = streamReader.ReadToEnd();

                    try
                    {
                        sosigTemplate = JsonUtility.FromJson<Custom_SosigEnemyTemplate>(json);
                    }
                    catch (System.Exception ex)
                    {
                        Debug.Log(ex.Message);
                        return;
                    }

                    //Directories
                    for (int x = 0; x < sosigTemplate.customSosig.Length; x++)
                    {
                        sosigTemplate.customSosig[x].directory = directories[i];
                    }

                    //Add to our collection
                    CustomSosigLoaderPlugin.customSosigs.Add(sosigTemplate.sosigEnemyID, sosigTemplate);

                    Debug.Log("Custom Sosig Loader - Loaded " + sosigTemplate.sosigEnemyID + " - " + sosigTemplate.displayName);
                }
            }
        }

        public static void LoadSosigMaterial(Custom_Sosig customSosig)
        {
            string filename = customSosig.directory + customSosig.customSkin;

            customSosig.albedo = LoadTexture(filename + ".png");
            customSosig.normalmap = LoadTexture(filename + "_Normal.png");
            customSosig.masr = LoadTexture(filename + "_MARS.png");
        }


        public static void ItemIDToList(string[] itemIDs, List<FVRObject> input)
        {
            for (int i = 0; i < itemIDs.Length; i++)
            {
                FVRObject mainObject;
                if (IM.OD.TryGetValue(itemIDs[i], out mainObject))
                    input.Add(mainObject);
                else
                    Debug.Log("Custom Sosig Loader - Could not find " + itemIDs);
            }
        }

        public static Texture2D LoadTexture(string path)
        {
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
                Debug.LogError("Custom Sosig Loader - Texture Not Found: " + path);
                return null;
            }
            return tex;
        }

        public static void LoadWhiteSosigTexture(string textureName)
        {
            //if(loadedTextures.Contains(textureName))

            string path = Paths.PluginPath + "\\Packer-Custom_Sosig_Loader\\" + textureName;
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
                Debug.LogError("Custom Sosig Loader - Texture Not Found: " + path);
                whiteSosig = null;
            }

            tex.name = textureName;

            whiteSosig = tex;
        }
    }
}
