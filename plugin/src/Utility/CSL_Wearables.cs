using UnityEngine;
using FistVR;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    public class CSL_Wearables : MonoBehaviour
    {
        public string[] Headwear;
        public string[] Torsowear;
        public string[] Pantswear;
        public string[] Pantswear_Lower;

        private Sosig sosig;

        void Awake()
        {
            sosig = transform.root.GetComponent<Sosig>();
        }

        void Start()
        {
            if (sosig != null)
                GenerateClothing();
            else
                CustomSosigLoaderPlugin.Logger.LogMessage($"Could not find Sosig Body");
        }

        void GenerateClothing()
        {
            SetupAccessories(Torsowear, sosig.Links[1]);
            SetupAccessories(Headwear, sosig.Links[0]);
            SetupAccessories(Pantswear, sosig.Links[2]);
            SetupAccessories(Pantswear_Lower, sosig.Links[3]);
        }

        void SetupAccessories(string[] items, SosigLink link)
        {
            List<FVRObject> wear = SetupItems(items);
            for (int i = 0; i < wear.Count; i++)
                SpawnAccessoryToLink(wear[i], link);
        }

        void SpawnAccessoryToLink(FVRObject item, SosigLink link)
        {
            if (item == null) 
                return;

            var go = item.GetGameObject();
            if (go == null) 
                return;

            var linkTransform = link.transform;
            var accessory = Object.Instantiate(go, linkTransform.position, linkTransform.rotation, linkTransform);
            accessory.GetComponent<SosigWearable>().RegisterWearable(link);
        }

        List<FVRObject> SetupItems(string[] itemIDs)
        {
            List<FVRObject> list = new List<FVRObject>();

            if (itemIDs == null || itemIDs.Length <= 0)
                return list;

            for (int i = 0; i < itemIDs.Length; i++)
            {
                if (IM.OD.ContainsKey(itemIDs[i]))
                {
                    list.Add(IM.OD[itemIDs[i]]);
                }
                else
                    CustomSosigLoaderPlugin.Logger.LogMessage($"Item ID not found: " + itemIDs);
            }

            return list;
        }
    }
}
