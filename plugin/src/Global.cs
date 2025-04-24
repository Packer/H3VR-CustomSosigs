using System.Collections.Generic;
using FistVR;
using UnityEngine;
using BepInEx;
using System;
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
                CustomSosigLoaderPlugin.Logger.LogInfo("No Custom Sosigs were found!");
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
                        CustomSosigLoaderPlugin.Logger.LogInfo(ex.Message);
                        return;
                    }

                    //Directories
                    for (int x = 0; x < sosigTemplate.CustomSosigs.Length; x++)
                    {
                        sosigTemplate.CustomSosigs[x].directory = directories[i];
                    }

                    //Add to our collection
                    CustomSosigLoaderPlugin.customSosigs.Add(sosigTemplate.SosigEnemyID, sosigTemplate);

                    CustomSosigLoaderPlugin.Logger.LogInfo("Custom Sosig Loader - Loaded " + sosigTemplate.SosigEnemyID + " - " + sosigTemplate.DisplayName);
                }
            }
        }

        public static void LoadSosigMaterial(Custom_Sosig customSosig)
        {
            //Using Default White skin so skip
            if (customSosig.customSkin == "CustomSosig_Base")
                return;

            string filename = Path.GetDirectoryName(customSosig.directory) + @"\" + customSosig.customSkin;

            customSosig.albedo = LoadTexture(filename + ".png");
            customSosig.normalmap = LoadTexture(filename + "_Normal.png");
            customSosig.masr = LoadTexture(filename + "_MARS.png");
        }

        public static void ItemIDToList(string[] itemIDs, List<FVRObject> input)
        {
            if (itemIDs == null)
            {
                CustomSosigLoaderPlugin.Logger.LogInfo("Item IDs missing");
                return;
            }

            if(input == null)
            {
                CustomSosigLoaderPlugin.Logger.LogInfo("Item IDs Input missing");
                return;
            }


            for (int i = 0; i < itemIDs.Length; i++)
            {
                FVRObject mainObject;
                if (IM.OD.ContainsKey(itemIDs[i]) && IM.OD.TryGetValue(itemIDs[i], out mainObject))
                    input.Add(mainObject);
                else
                    CustomSosigLoaderPlugin.Logger.LogInfo("Custom Sosig Loader - Could not find |" + itemIDs[i] + "|");
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
                CustomSosigLoaderPlugin.Logger.LogWarning("Custom Sosig Loader - Texture Not Found: " + path);
                return null;
            }
            return tex;
        }

        public static void LoadWhiteSosigTexture(string textureName)
        {
            //if(loadedTextures.Contains(textureName))

            string path = Paths.PluginPath + "\\Sosig_Squad-Custom_Sosig_Loader\\" + textureName;
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
                CustomSosigLoaderPlugin.Logger.LogError("Custom Sosig Loader - Texture Not Found: " + path);
                whiteSosig = null;
            }

            tex.name = textureName;

            whiteSosig = tex;
        }


        public static void LoadCustomVoiceLines()
        {
            CustomSosigLoaderPlugin.Logger.LogInfo("Loading Custom Voice Lines");

            try
            {
                // Get all directories in the root directory
                string[] directories = Directory.GetDirectories(Paths.PluginPath, "*", SearchOption.AllDirectories);

                List<string> foundVoices = new List<string>();

                foreach (string directory in directories)
                {
                    // Check if the directory contains a "voicelines" folder
                    string voicelinesPath = Path.Combine(directory, "voicelines");
                    if (Directory.Exists(voicelinesPath))
                    {
                        // Check if the directory contains a .yaml file
                        string[] yamlFiles = Directory.GetFiles(directory, "*.yaml", SearchOption.TopDirectoryOnly);
                        if (yamlFiles.Length > 0)
                        {
                            // Process each .yaml file
                            foreach (string yamlFile in yamlFiles)
                            {
                                // Open the .yaml file and extract the VoicelineName
                                VoiceSet set = GetVoicelines(yamlFile);
                                string voicelineName = set.name;
                                float voicePitch = set.pitch;
                                float voiceVolume = set.volume;

                                if (!string.IsNullOrEmpty(voicelineName) && !foundVoices.Contains(voicelineName))
                                {
                                    SosigSpeechSet speech = ScriptableObject.CreateInstance<SosigSpeechSet>();
                                    speech.name = voicelineName;
                                    speech.BasePitch = voicePitch;
                                    speech.BaseVolume = voiceVolume;

                                    speech.OnAssault = new List<AudioClip>();
                                    speech.OnBackBreak = new List<AudioClip>();
                                    speech.OnBeingAimedAt = new List<AudioClip>();
                                    speech.OnCall_Assistance = new List<AudioClip>();
                                    speech.OnCall_Skirmish = new List<AudioClip>();
                                    speech.OnConfusion = new List<AudioClip>();
                                    speech.OnDeath = new List<AudioClip>();
                                    speech.OnDeathAlt = new List<AudioClip>();
                                    speech.OnInvestigate = new List<AudioClip>();
                                    speech.OnJointBreak = new List<AudioClip>();
                                    speech.OnJointSever = new List<AudioClip>();
                                    speech.OnJointSlice = new List<AudioClip>();
                                    speech.OnMedic = new List<AudioClip>();
                                    speech.OnNeckBreak = new List<AudioClip>();
                                    speech.OnPain = new List<AudioClip>();
                                    speech.OnReloading = new List<AudioClip>();
                                    speech.OnRespond_Assistance = new List<AudioClip>();
                                    speech.OnRespond_Skirmish = new List<AudioClip>();
                                    speech.OnSearchingForGuns = new List<AudioClip>();
                                    speech.OnSkirmish = new List<AudioClip>();
                                    speech.OnTakingCover = new List<AudioClip>();
                                    speech.OnWander = new List<AudioClip>();
                                    speech.Test = new List<AudioClip>();

                                    foundVoices.Add(voicelineName);

                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "state_wander"), speech.OnWander);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "state_skirmish"), speech.OnSkirmish);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "state_reload"), speech.OnReloading);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "state_investigate"), speech.OnInvestigate);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_joint_slice"), speech.OnJointSlice);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_joint_sever"), speech.OnJointSever);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_joint_break"), speech.OnJointBreak);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_default"), speech.OnPain);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_break_neck"), speech.OnNeckBreak);
                                    AddAllAudioFromDirectory(Path.Combine(voicelinesPath, "pain_break_back"), speech.OnBackBreak);

                                    CustomSosigLoaderPlugin.customVoicelines.Add(voicelineName, speech);

                                    CustomSosigLoaderPlugin.Logger.LogInfo("Custom Sosig Loader: Loaded voicelines - " + voicelineName);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public static void AddAllAudioFromDirectory(string path, List<AudioClip> list)
        {
            // Load .wav files in each subdirectory
            string[] wavFiles = Directory.GetFiles(path, "*.wav", SearchOption.TopDirectoryOnly);
            foreach (string wavFile in wavFiles)
            {
                // Load the .wav file as an AudioClip
                AudioClip clip = LoadWav(wavFile);
                if (clip != null)
                {
                    list.Add(clip);
                }
            }
        }

        public static AudioClip LoadWav(string filePath)
        {
            try
            {
                return WavUtility.ToAudioClip(filePath);
            }
            catch (Exception ex)
            {
                CustomSosigLoaderPlugin.Logger.LogError("An error occurred while loading the .wav file: " + ex.Message);
                return null;
            }
        }

        static VoiceSet GetVoicelines(string yamlFilePath)
        {
            try
            {
                VoiceSet newSet = new VoiceSet();

                // Read all lines from the .yaml file
                string[] lines = File.ReadAllLines(yamlFilePath);

                // Search for the line containing "name:"
                foreach (string line in lines)
                {
                    if (line.Trim().StartsWith("name:"))
                    {
                        // Name
                        newSet.name = line.Substring(line.IndexOf("name: ") + "name:".Length).Trim();
                    }

                    if (line.Trim().StartsWith("base_pitch:"))
                    {
                        // Pitch
                        newSet.pitch = float.Parse(line.Substring(line.IndexOf("base_pitch: ") + "base_pitch:".Length).Trim());
                    }

                    if (line.Trim().StartsWith("base_volume:"))
                    {
                        // Volume
                        newSet.volume = float.Parse(line.Substring(line.IndexOf("base_volume: ") + "base_volume:".Length).Trim());
                    }
                }

                return newSet;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Custom Sosig Loader: An error occurred while reading the file: " + ex.Message);
            }

            return null;
        }


        public static Sosig GetSosig(Transform start)
        {
            Sosig sosig = start.root.GetComponent<Sosig>();

            if (sosig == null)
            {
                SosigWearable wearable = start.GetComponent<SosigWearable>();
                if (wearable != null)
                    sosig = wearable.S;
            }

            if (sosig == null)
                sosig = start.GetComponent<Sosig>();

            return sosig;
        }


        public static Quaternion SmoothRotateTowards(Vector3 direction, Quaternion currentRotation, float turnSpeed)
        {
            Quaternion quaternionDirection = Quaternion.LookRotation(direction);
            return Quaternion.RotateTowards(currentRotation, quaternionDirection, turnSpeed * Time.deltaTime);
        }
    }

    public class VoiceSet
    {
        public string name = "";
        public float pitch = 1.15f;
        public float volume = 0.4f;
    }
}
