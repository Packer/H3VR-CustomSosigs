using TNHFramework;
using FistVR;
using BepInEx;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    public class TnHFrameworkLoader
    {
        public static void AddSosigEnemyTemplateToTnHFramework(SosigEnemyTemplate template)
        {
            LoadedTemplateManager.SosigIDDict.Add(template.DisplayName, (int)template.SosigEnemyID);
        }

        public class FrameworkSosig
        {
            public string DisplayName;
            public string SosigEnemyCategory;
            public string SosigEnemyID;
            public string[] SosigPrefabs;
            public Custom_SosigConfigTemplate[] Configs;
            public Custom_SosigConfigTemplate[] ConfigsEasy;

        }

        public static void LoadFrameworkSosigs()
        {
            CustomSosigLoaderPlugin.Logger.LogInfo("Loading TnH Framework Sosigs");

            try
            {
                // Get all directories in the root directory
                string[] directories = Directory.GetDirectories(Paths.PluginPath, "*", SearchOption.AllDirectories);

                List<string> foundSosigs = new List<string>();

                foreach (string directory in directories)
                {
                    // Check if the directory contains a "voicelines" folder
                    string sosigsPath = Path.Combine(directory, "Sosigs");
                    if (Directory.Exists(sosigsPath))
                    {
                        // Check if the directory contains a .yaml file
                        string[] jsonFiles = Directory.GetFiles(directory, "*.json", SearchOption.TopDirectoryOnly);
                        if (jsonFiles.Length > 0)
                        {
                            // Process each .json file
                            foreach (string json in jsonFiles)
                            {
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
    }
}