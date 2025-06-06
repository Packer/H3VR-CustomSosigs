﻿using BepInEx;
using BepInEx.Logging;
using FistVR;
using System.Collections.Generic;
using UnityEngine;
using BepInEx.Bootstrap;
using OtherLoader.Loaders;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace CustomSosigLoader
{
    [BepInPlugin("Sosig_Squad.CustomSosigLoader", "Custom Sosig Loader", "1.0.6")]
    [BepInProcess("h3vr.exe")]
    [BepInDependency("VIP.TommySoucy.H3MP", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Sosig_Squad.SupplyRaid", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("h3vr.otherloader", BepInDependency.DependencyFlags.HardDependency)]
    //[BepInDependency("h3vr.tnhframework", BepInDependency.DependencyFlags.SoftDependency)]
    public partial class CustomSosigLoaderPlugin : BaseUnityPlugin
    {
        public static bool h3mpEnabled = false;
        public static bool supplyRaidEnabled = false;
        public static bool otherLoaderEnabled = false;
        public static bool tnhFramework = false;

        public static bool CustomSosigsLoaded = false;

        public static SosigMP sosigMP;

        public static Dictionary<int, Custom_SosigEnemyTemplate> customSosigs = new Dictionary<int, Custom_SosigEnemyTemplate>();
        public static Dictionary<SosigConfigTemplate, SosigEnemyID> customSosigConfigs = new Dictionary<SosigConfigTemplate, SosigEnemyID>();
        public static Dictionary<SosigEnemyID, SosigConfigTemplate> customSosigIDs = new Dictionary<SosigEnemyID, SosigConfigTemplate>();
        public static Texture2D customSosigTexture;

        public static Dictionary<string, SosigSpeechSet> customVoicelines = new Dictionary<string, SosigSpeechSet>();
        //private AnvilCallback<GameObject> _sosigData;

        public static bool screenshotSosigs = false;
        public static bool screenshotSosigGear = false;


        public delegate void SosigsLoadedComplete();
        public static event SosigsLoadedComplete SosigsLoadedCompleted;

        public static int vehicleAgentID = 1;

        private void Awake()
        {
            Logger = base.Logger;
            //Enabled Mods for soft dependencies
            h3mpEnabled = Chainloader.PluginInfos.ContainsKey("VIP.TommySoucy.H3MP");
            supplyRaidEnabled = Chainloader.PluginInfos.ContainsKey("com.Packer.SupplyRaid");
            otherLoaderEnabled = Chainloader.PluginInfos.ContainsKey("h3vr.otherloader");
            tnhFramework = Chainloader.PluginInfos.ContainsKey("h3vr.tnhframework");

            //Wait for all custom items to be added before loading more
            OtherLoader.LoaderStatus.ProgressUpdated += LoadCustomSosigCheck;

            //SceneManager.activeSceneChanged += ChangedActiveScene;
        }

        void Start()
        {
            if (h3mpEnabled)
            {
                sosigMP = new GameObject().AddComponent<SosigMP>();
                DontDestroyOnLoad(sosigMP.gameObject);
            }

        }

        void OnDestroy()
        {
            OtherLoader.LoaderStatus.ProgressUpdated -= LoadCustomSosigCheck;
        }

        void LoadCustomSosigs()
        {
            Logger.LogInfo("Custom Sosig Loader: Loading Sosigs");
            Logger.LogInfo("Custom Sosig Loader: Start Sosig Capture - Right Shift + O");
            Logger.LogInfo("Custom Sosig Loader: Stop Sosig Capture - Right Shift + P");
            Logger.LogInfo("Custom Sosig Loader: Start Gear Capture - Right Shift + U");
            Logger.LogInfo("Custom Sosig Loader: Stop Gear Capture - Right Shift + I");
            Global.LoadCustomVoiceLines();
            Global.LoadWhiteSosigTexture("CustomSosig_Base.png");
            Global.LoadCustomSosigs();
            StartCoroutine(SetupSosigTemplates());
            HarmonyLib.Harmony.CreateAndPatchAll(typeof(Hooks));
        }

        private void LoadCustomSosigCheck(float progress)
        {
            if (CustomSosigsLoaded || progress < 1f)
                return;

            //Load our intial sosigs
            LoadCustomSosigs();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.T))
            {
                Logger.LogInfo("Printing all Sosig Clothing");
                PrintAllFVRObjects();
            }
            

            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.O))
            {
                Logger.LogInfo("Right Shift + O Pressed - Start Sosig Capture");
                if (!screenshotSosigs)
                {
                    screenshotSosigs = true;
                    StartCoroutine(SosigScreenshots.RunSosigCapture());
                }
            }
            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.P))
            {
                Logger.LogInfo("Right Shift + P Pressed - Stop Sosig Capture");
                screenshotSosigs = false;
            }

            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.U))
            {
                Logger.LogInfo("Right Shift + U Pressed - Start Gear Capture");
                if (!screenshotSosigGear)
                {
                    screenshotSosigGear = true;
                    StartCoroutine(SosigScreenshots.RunSosigGearCapture());
                }
            }
            if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyUp(KeyCode.I))
            {
                Logger.LogInfo("Right Shift + I Pressed - Stop Gear Capture");
                screenshotSosigGear = false;
            }
        }

        public System.Collections.IEnumerator SetupSosigTemplates()
        {
            Logger.LogInfo("Custom Sosigs count: " + customSosigs.Count);
            Logger.LogInfo("Sosigs count: " + IM.Instance.odicSosigObjsByID.Count);

            foreach (var customTemplate in customSosigs)
            {
                //SR_SosigEnemyTemplate customTemplate = SupplyRaidPlugin.customSosigs.Ele;
                SosigEnemyTemplate template = customTemplate.Value.Initialize();

                template.DisplayName = customTemplate.Value.DisplayName;
                template.SosigEnemyID = (SosigEnemyID)customTemplate.Value.SosigEnemyID;
                template.SosigEnemyCategory = (SosigEnemyCategory)customTemplate.Value.SosigEnemyCategory;

                template.SosigPrefabs = new List<FVRObject>();

                List<string> customSkins = new List<string>();

                for (int i = 0; i < customTemplate.Value.CustomSosigs.Length; i++)
                {
                    //Get our Base Sosig
                    SosigEnemyID id = customTemplate.Value.CustomSosigs[i].baseSosigID;
                    template.SosigPrefabs = IM.Instance.odicSosigObjsByID[id].SosigPrefabs;

                    //Sosig Texture
                    if (customTemplate.Value.CustomSosigs[i].customSkin != "")
                    {
                        Global.LoadSosigMaterial(customTemplate.Value.CustomSosigs[i]);
                    }
                }

                //Custom Sosigs
                for (int i = 0; i < template.ConfigTemplates.Count; i++)
                {
                    if (!customSosigConfigs.ContainsKey(template.ConfigTemplates[i]))
                        customSosigConfigs.Add(template.ConfigTemplates[i], template.SosigEnemyID);

                    if (!customSosigIDs.ContainsKey(template.SosigEnemyID))
                        customSosigIDs.Add(template.SosigEnemyID, template.ConfigTemplates[i]);
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
            Logger.LogInfo("Sosigs Total count: " + IM.Instance.odicSosigObjsByID.Count);

            //Finished Loading everything!
            CustomSosigsLoaded = true;
            if (SosigsLoadedCompleted != null)
                SosigsLoadedCompleted.Invoke();
        }

        public void PrintAllFVRObjects()
        {
            //Loop through every item in the game and compare Keyword
            foreach (string key in IM.OD.Keys)
            {
                if (IM.OD.TryGetValue(key, out FVRObject fvrObject))
                {
                    if (fvrObject && fvrObject.Category == FVRObject.ObjectCategory.SosigClothing)
                    {
                        Logger.LogInfo(fvrObject.DisplayName);
                    }
                }
            }
        }

        // --------------------------------------------------------------------------------
        // VEHICLE NAV MESH
        // --------------------------------------------------------------------------------
        /*
        void OnDisable()
        {
            SceneManager.activeSceneChanged -= ChangedActiveScene;
        }

        public static NavMeshSurface sceneSurface;

        private void ChangedActiveScene(Scene current, Scene next)
        {
            SetupNavMesh();
        }

        int SetupNavMesh()
        {
            Debug.Log("Build START " + Time.time);
            //Build Navmesh for vehicles here if there isn't one in this scene
            NavMeshBuildSettings sosigSettings = NavMesh.GetSettingsByIndex(0);

            NavMeshBuildSettings vehicleSettings = NavMesh.CreateSettings();
            vehicleSettings.agentHeight = 3;
            vehicleSettings.agentRadius = 3;
            vehicleSettings.agentClimb = sosigSettings.agentClimb;
            vehicleSettings.agentSlope = sosigSettings.agentSlope;

            GameObject navObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            sceneSurface = new GameObject().AddComponent<NavMeshSurface>();
            sceneSurface.m_UseGeometry = NavMeshCollectGeometry.PhysicsColliders;
            //sceneSurface.layerMask = LayerMask.NameToLayer("Environment");
            sceneSurface.BuildNavMesh();

            Debug.Log("AGENT ID: " + vehicleSettings.agentTypeID);
            Debug.Log("Build Complete: " + Time.time);
            return vehicleSettings.agentTypeID;
        }
        */

        // The line below allows access to your plugin's logger from anywhere in your code, including outside of this file.
        // Use it with 'YourPlugin.Logger.LogInfo(message)' (or any of the other Log* methods)
        internal new static ManualLogSource Logger { get; private set; }
    }
}
