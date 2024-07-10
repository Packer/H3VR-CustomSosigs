using BepInEx;
using BepInEx.Logging;
using FistVR;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BepInEx.Bootstrap;

namespace CustomSosigLoader
{
    [BepInPlugin("com.Packer.CustomSosigLoader", "Custom Sosig Loader", "1.0.0")]
    [BepInProcess("h3vr.exe")]
    [BepInDependency("VIP.TommySoucy.H3MP", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Packer.SupplyRaid", BepInDependency.DependencyFlags.SoftDependency)]
    public partial class CustomSosigLoaderPlugin : BaseUnityPlugin
    {
        public static bool h3mpEnabled = false;
        public static bool supplyRaidEnabled = false;

        //public static Dictionary<int, Custom_SosigEnemyTemplate> customSosigs = new Dictionary<int, Custom_SosigEnemyTemplate>();
        //public static List<Custom_SosigEnemyTemplate> customSosigs = new List<Custom_SosigEnemyTemplate>();
        public static Dictionary<int, Custom_SosigEnemyTemplate> customSosigs = new Dictionary<int, Custom_SosigEnemyTemplate>();
        public static Dictionary<SosigConfigTemplate, SosigEnemyID> customSosigConfigs = new Dictionary<SosigConfigTemplate, SosigEnemyID>();
        public static Texture2D customSosigTexture;
        //private AnvilCallback<GameObject> _sosigData;


        private void Awake()
        {
            Logger = base.Logger;
            h3mpEnabled = Chainloader.PluginInfos.ContainsKey("VIP.TommySoucy.H3MP");
            supplyRaidEnabled = Chainloader.PluginInfos.ContainsKey("com.Packer.SupplyRaid");

            // Your plugin's ID, Name, and Version are available here.
            //Logger.LogMessage($"Hello, world! Sent from {Id} {Name} {Version}");

        }

        void Start()
        {
            Logger.LogInfo("Custom Sosig Loader: Loading Sosigs");
            Global.LoadCustomSosigTexture("CustomSosig_Base.png");
            Global.LoadCustomSosigs();
            StartCoroutine(SetupSosigTemplates());
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Hooks), (string)null);
        }

        public System.Collections.IEnumerator SetupSosigTemplates()
        {
            Debug.Log("Custom Sosigs count: " + customSosigs.Count);
            Debug.Log("Sosigs count: " + IM.Instance.odicSosigObjsByID.Count);

            foreach (var customTemplate in customSosigs)
            {
                //SR_SosigEnemyTemplate customTemplate = SupplyRaidPlugin.customSosigs.Ele;
                SosigEnemyTemplate template = customTemplate.Value.Initialize();

                template.SosigEnemyID = (SosigEnemyID)customTemplate.Value.sosigEnemyID;
                template.SosigEnemyCategory = (SosigEnemyCategory)customTemplate.Value.sosigEnemyCategory;

                template.SosigPrefabs = new List<FVRObject>();

                for (int i = 0; i < customTemplate.Value.customSosig.Length; i++)
                {
                    //Get our Base Sosig
                    SosigEnemyID id = customTemplate.Value.customSosig[i].baseSosigID;
                    template.SosigPrefabs = IM.Instance.odicSosigObjsByID[id].SosigPrefabs;
                }

                //Custom Sosigs
                for (int i = 0; i < template.ConfigTemplates.Count; i++)
                {
                    if (!customSosigConfigs.ContainsKey(template.ConfigTemplates[i]))
                    {
                        customSosigConfigs.Add(template.ConfigTemplates[i], template.SosigEnemyID);
                    }
                }


                //Assign to Base Game Sosig Collections
                if (!IM.Instance.olistSosigCats.Contains(template.SosigEnemyCategory))
                {
                    //Adding Category
                    IM.Instance.olistSosigCats.Add(template.SosigEnemyCategory);
                }
                if (!IM.Instance.odicSosigIDsByCategory.ContainsKey(template.SosigEnemyCategory))
                {
                    List<SosigEnemyID> sosigIDs = new List<SosigEnemyID>();
                    IM.Instance.odicSosigIDsByCategory.Add(template.SosigEnemyCategory, sosigIDs);
                    List<SosigEnemyTemplate> enemyTemplates = new List<SosigEnemyTemplate>();
                    IM.Instance.odicSosigObjsByCategory.Add(template.SosigEnemyCategory, enemyTemplates);
                }
                if (template.SosigEnemyID != SosigEnemyID.None)
                {
                    IM.Instance.odicSosigIDsByCategory[template.SosigEnemyCategory].Add(template.SosigEnemyID);
                    IM.Instance.odicSosigObjsByCategory[template.SosigEnemyCategory].Add(template);
                    if (!IM.Instance.odicSosigObjsByID.ContainsKey(template.SosigEnemyID))
                    {
                        IM.Instance.odicSosigObjsByID.Add(template.SosigEnemyID, template);
                    }
                }
                yield return null;
            }
            Debug.Log("Sosigs count: " + IM.Instance.odicSosigObjsByID.Count);
        }


        // The line below allows access to your plugin's logger from anywhere in your code, including outside of this file.
        // Use it with 'YourPlugin.Logger.LogInfo(message)' (or any of the other Log* methods)
        internal new static ManualLogSource Logger { get; private set; }
    }
}
