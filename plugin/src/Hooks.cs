﻿using FistVR;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Packer
{
    [HarmonyPatch]
    internal static class Hooks
    {
        [HarmonyPatch(typeof(Sosig)), HarmonyPatch(nameof(Sosig.Configure)), HarmonyPrefix]
        public static void Configure_Prefix(Sosig __instance, SosigConfigTemplate t)
        {
            Debug.Log(" ATHIS AJHJSNA D JLASDK JLKSDAB JBLKSAD JLBKDSAJ BLKDASB LJDSALBJ ADSL JBADS");
            //Get Sosig ID if its custom Sosig
            if (CustomSosigLoaderPlugin.customSosigConfigs.TryGetValue(t, out SosigEnemyID id))
            {
                //Get Custom Sosig Template
                if (CustomSosigLoaderPlugin.customSosigs.TryGetValue((int)id, out Custom_SosigEnemyTemplate template))
                {
                    ModifySosig(__instance, template);
                }
            }
        }


        public static void ModifySosig(Sosig sosig, Custom_SosigEnemyTemplate template)
        {
            if (template == null)
                Debug.Log("Custom Sosig Loader: Missing Template");

            Custom_Sosig custom = template.customSosig[Random.Range(0, template.customSosig.Length)];
            Custom_SosigConfigTemplate config = template.configTemplates[Random.Range(0, template.configTemplates.Length)];
            bool stopSever = config.CanBeSevered;

            if (!stopSever)
            {
                if (custom.scaleHead.y * custom.scaleBody.y > 1
                    || custom.scaleTorso.y * custom.scaleBody.y > 1
                    || custom.scaleLegsUpper.y * custom.scaleBody.y > 1
                    || custom.scaleLegsLower.y * custom.scaleBody.y > 1)
                    stopSever = true;
            }

            //Sosig Material Changes
            MeshRenderer head = Custom_SosigData.GetSosigMeshRenderer("Geo_Head", sosig.Links[0].transform);
            Material sosigMaterial = head.material;
            if (custom.useCustomSkin)
                sosigMaterial.SetTexture("_MainTex", CustomSosigLoaderPlugin.customSosigTexture);
            sosigMaterial.SetColor("_Color", custom.color);
            sosigMaterial.SetFloat("_Metallic", custom.metallic);
            sosigMaterial.SetFloat("_Specularity", custom.specularity);
            sosigMaterial.SetFloat("_SpecularTint", custom.specularTint);
            sosigMaterial.SetFloat("_Roughness", custom.roughness);
            sosigMaterial.SetFloat("_BumpScale", custom.normalStrength);
            sosigMaterial.SetInt("_SpecularHighlights", custom.specularHighlights ? 1 : 0);
            sosigMaterial.SetInt("_GlossyReflections", custom.glossyReflections ? 1 : 0);
            head.sharedMaterial = sosigMaterial;
            sosig.GibMaterial = sosigMaterial;

            //Head
            if (sosig.Links.Count >= 1)
                Custom_SosigData.UpdateSosigLink(sosig.Links[0], custom.scaleBody, custom.scaleHead, sosigMaterial, stopSever, (MeshRenderer)sosig.Renderers[0]);

            //Torso
            if (sosig.Links.Count >= 2)
                Custom_SosigData.UpdateSosigLink(sosig.Links[1], custom.scaleBody, custom.scaleTorso, sosigMaterial, stopSever, (MeshRenderer)sosig.Renderers[1]);

            //Legs
            if (sosig.Links.Count >= 3)
                Custom_SosigData.UpdateSosigLink(sosig.Links[2], custom.scaleBody, custom.scaleLegsUpper, sosigMaterial, stopSever, (MeshRenderer)sosig.Renderers[2]);

            //Legs Lower
            if (sosig.Links.Count >= 4)
                Custom_SosigData.UpdateSosigLink(sosig.Links[3], custom.scaleBody, custom.scaleLegsUpper, sosigMaterial, stopSever, (MeshRenderer)sosig.Renderers[3]);

            //Overall Scale
            sosig.transform.localScale = custom.scaleBody;

            //Update Agent
            float agentRadius = (custom.scaleBody.x > custom.scaleBody.z ? custom.scaleBody.x : custom.scaleBody.z);
            if (agentRadius > sosig.Agent.radius)
                sosig.Agent.radius = sosig.Agent.radius * (custom.scaleBody.x > custom.scaleBody.z ? custom.scaleBody.x : custom.scaleBody.z);

            sosig.Agent.height *= custom.scaleBody.y;

            //ANTON PLEASSS
            SosigSpeechSet speechSet = ScriptableObject.CreateInstance<SosigSpeechSet>();

            speechSet.OnAssault = sosig.Speech.OnAssault;
            speechSet.OnAssault = sosig.Speech.OnAssault;
            speechSet.OnBackBreak = sosig.Speech.OnBackBreak;
            speechSet.OnBeingAimedAt = sosig.Speech.OnBeingAimedAt;
            speechSet.OnCall_Assistance = sosig.Speech.OnCall_Assistance;
            speechSet.OnCall_Skirmish = sosig.Speech.OnCall_Skirmish;
            speechSet.OnConfusion = sosig.Speech.OnConfusion;
            speechSet.OnDeath = sosig.Speech.OnDeath;
            speechSet.OnDeathAlt = sosig.Speech.OnDeathAlt;
            speechSet.OnInvestigate = sosig.Speech.OnInvestigate;
            speechSet.OnJointBreak = sosig.Speech.OnJointBreak;
            speechSet.OnJointSever = sosig.Speech.OnJointSever;
            speechSet.OnJointSlice = sosig.Speech.OnJointSlice;
            speechSet.OnMedic = sosig.Speech.OnMedic;
            speechSet.OnNeckBreak = sosig.Speech.OnNeckBreak;
            speechSet.OnPain = sosig.Speech.OnPain;
            speechSet.OnReloading = sosig.Speech.OnReloading;
            speechSet.OnRespond_Assistance = sosig.Speech.OnRespond_Assistance;
            speechSet.OnRespond_Skirmish = sosig.Speech.OnRespond_Skirmish;
            speechSet.OnSearchingForGuns = sosig.Speech.OnSearchingForGuns;
            speechSet.OnSkirmish = sosig.Speech.OnSkirmish;
            speechSet.OnTakingCover = sosig.Speech.OnTakingCover;
            speechSet.OnWander = sosig.Speech.OnWander;
            speechSet.Test = sosig.Speech.Test;

            speechSet.LessTalkativeSkirmish = sosig.Speech.LessTalkativeSkirmish;
            speechSet.UseAltDeathOnHeadExplode = sosig.Speech.UseAltDeathOnHeadExplode;
            speechSet.ForceDeathSpeech = sosig.Speech.ForceDeathSpeech;

            speechSet.BasePitch = custom.voicePitch;
            speechSet.BaseVolume = custom.voiceVolume;

            sosig.Speech = speechSet;

        }
    }
}