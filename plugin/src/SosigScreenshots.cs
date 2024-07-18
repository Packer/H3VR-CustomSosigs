using System.Collections.Generic;
using System.Collections;
using System.Linq;
using FistVR;
using UnityEngine;
using BepInEx;
using System;
using System.IO;
using Sodalite.Api;
using UnityEngine.AI;

namespace CustomSosigLoader
{
    public class SosigScreenshots
    {
        public static Camera captureCamera;
        public static Transform spawnPoint;
        public static Sosig currentSosig;
        public static GameObject currentGear;

        public static readonly SosigAPI.SpawnOptions _spawnOptions = new SosigAPI.SpawnOptions
        {
            SpawnState = Sosig.SosigOrder.Disabled,
            SpawnActivated = true,
            EquipmentMode = SosigAPI.SpawnOptions.EquipmentSlots.All,
            SpawnWithFullAmmo = true,
            IFF = 0,
        };

        public static void CreateScreenshotSetup()
        {
            if (captureCamera != null)
                return;

            GameObject cameraObject = new GameObject();
            cameraObject.transform.position = Vector3.up * 150;
            captureCamera = cameraObject.AddComponent<Camera>();
            captureCamera.fov = 15;
            captureCamera.fieldOfView = 15;
            captureCamera.targetTexture = new RenderTexture(1024, 1024, 1, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            captureCamera.Render();

            spawnPoint = new GameObject().transform;
            spawnPoint.parent = cameraObject.transform;
            spawnPoint.localPosition = new Vector3(0, -1, 10);
            spawnPoint.rotation = Quaternion.Euler(0, 180, 0);

            Material material = new Material(Shader.Find("Unlit/Color"));
            material.SetColor("_Color", Color.green);

            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.GetComponent<MeshRenderer>().material = material;
            wall.transform.parent = spawnPoint.transform;
            wall.transform.localPosition = new Vector3(0, 0, -2);
            wall.transform.localScale = new Vector3(10, 10, 1);

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.GetComponent<MeshRenderer>().material = material;
            ground.transform.parent = spawnPoint;
            ground.transform.localPosition = new Vector3(0, -0.51f, 0);
            ground.transform.localScale = new Vector3(10, 1, 10);
            ground.AddComponent<NavMeshSurface>().BuildNavMesh();

            GameObject light = new GameObject();
            Light newLight = light.AddComponent<Light>();
            newLight.intensity = 4;
            newLight.type = LightType.Point;
            newLight.range = 30;
            light.transform.parent = spawnPoint;
            light.transform.localPosition = new Vector3(1.5f, 3, 3);
        }
        
        public static List<FVRObject> GearSosigClothing()
        {
            List<FVRObject> gearIDs = new List<FVRObject>();

            //Loop through every item in the game and compare Keyword
            foreach (string key in IM.OD.Keys)
            {
                if (IM.OD.TryGetValue(key, out FVRObject fvrObject))
                {
                    if (fvrObject && fvrObject.Category == FVRObject.ObjectCategory.SosigClothing)
                    {
                        gearIDs.Add(fvrObject);
                    }
                }
            }

            return gearIDs;
        }
        public static IEnumerator RunSosigGearCapture()
        {
            CreateScreenshotSetup();
            List<FVRObject> gearList = GearSosigClothing();
            yield return null;

            captureCamera.fov = 7.5f;
            captureCamera.fieldOfView = 7.5f;

            CustomSosigLoaderPlugin.screenshotSosigGear = true;
            CustomSosigLoaderPlugin.Logger.LogInfo("Sosig Gear Started at " + DateTime.Now);
            foreach (FVRObject gear in gearList)
            {
                if (!CustomSosigLoaderPlugin.screenshotSosigGear)
                {
                    CustomSosigLoaderPlugin.Logger.LogInfo("Sosig Gear force stopped at " + DateTime.Now);
                    if (currentGear)
                        GameObject.Destroy(currentGear);
                    yield break;
                }

                //Create Gear
                GameObject spawnObject = gear.GetGameObject();
                if(spawnObject)
                    currentGear = UnityEngine.Object.Instantiate(spawnObject).gameObject;

                if (spawnObject == null || currentGear == null)
                    continue;

                currentGear.transform.position = spawnPoint.position + Vector3.up;
                currentGear.transform.rotation = Quaternion.Euler(0, 180, 0);
                //currentGear.GetComponent<Rigidbody>().isKinematic = true;

                //currentSosig.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

                yield return null;
                yield return null;
                yield return null;
                //Screenshot
                TakeScreenshot(gear.ItemID, "/Gear/");

                yield return null;

                if (currentGear)
                    GameObject.Destroy(currentGear);

                yield return null;
            }

            if (currentGear)
                GameObject.Destroy(currentGear);

            CustomSosigLoaderPlugin.Logger.LogInfo("Sosig Gear Complete at " + DateTime.Now);
        }


        public static IEnumerator RunSosigCapture()
        {
            CreateScreenshotSetup();
            yield return null;

            captureCamera.fov = 15;
            captureCamera.fieldOfView = 15;

            CustomSosigLoaderPlugin.screenshotSosigs = true;
            CustomSosigLoaderPlugin.Logger.LogInfo("Screen Capture Started at " + DateTime.Now);
            foreach (SosigEnemyID pieceType in Enum.GetValues(typeof(SosigEnemyID)))
            {
                if (!CustomSosigLoaderPlugin.screenshotSosigs)
                {
                    CustomSosigLoaderPlugin.Logger.LogInfo("Screen Capture force stopped at " + DateTime.Now);
                    if(currentSosig)
                        currentSosig.DeSpawnSosig();
                    yield break;
                }

                //Create Sosig

                string enumName = Enum.GetName(typeof(SosigEnemyID), pieceType);
                CustomSosigLoaderPlugin.Logger.LogInfo(pieceType + " - " + enumName);

                SosigEnemyTemplate newSosig;
                IM.Instance.odicSosigObjsByID.TryGetValue(pieceType, out newSosig);

                if (newSosig == null)
                    continue;

                currentSosig =
                    SosigAPI.Spawn(
                        newSosig,
                        _spawnOptions,
                        spawnPoint.position,
                        spawnPoint.rotation);

                if (currentSosig == null)
                    continue;
                //currentSosig.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;

                yield return null;
                yield return null;
                yield return null;
                //Screenshot
                TakeScreenshot((int)pieceType + "_" + enumName, "/Sosigs/");

                yield return null;
                yield return null;

                currentSosig.DeSpawnSosig();

                yield return null;
                yield return null;
            }
            if(captureCamera)
                GameObject.Destroy(captureCamera.gameObject);
            CustomSosigLoaderPlugin.Logger.LogInfo("Screen Capture Complete at " + DateTime.Now);
        }

        public static void TakeScreenshot(string nameID, string directory)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = captureCamera.targetTexture;
            captureCamera.Render();


            Texture2D imageOverview = new Texture2D(
                captureCamera.targetTexture.width, 
                captureCamera.targetTexture.height,
                TextureFormat.RGBA32,
                false,
                true);

            imageOverview.ReadPixels(new Rect(0, 0, captureCamera.targetTexture.width, captureCamera.targetTexture.height), 0, 0);
            imageOverview.Apply();
            RenderTexture.active = currentRT;

            // Encode texture into PNG
            byte[] bytes = imageOverview.EncodeToPNG();

            // save in memory
            string filename = nameID + ".png";
            string path = Application.persistentDataPath + directory;

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                CustomSosigLoaderPlugin.Logger.LogInfo(ex.Message);
                return;
            }

            path += filename;
            //File.WriteAllBytes(path, bytes);

            using (FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate))
            {
                CustomSosigLoaderPlugin.Logger.LogInfo("Write to file: " + path);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
