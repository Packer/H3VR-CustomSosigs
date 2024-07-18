using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomSosigUI : MonoBehaviour
{
    public static CustomSosigUI instance;

    public Custom_Sosig customSosig;

    [Header("Base")]
    public InputField nameField;
    public InputField baseSosigID;
    //public string customTextureName = "";

    [Header("Voice")]
    public InputField voiceSet;
    public InputField voiceVolume;
    public InputField voicePitch;

    [Header("Scale")]
    public InputField[] scaleBody = new InputField[2];     //Width and Height (Don't split x and z)
    public InputField[] scaleHead = new InputField[2];
    public InputField[] scaleTorso = new InputField[2];
    public InputField[] scaleLegsUpper = new InputField[2];
    public InputField[] scaleLegsLower = new InputField[2];

    [Header("Materials")]
    public Dropdown useCustomSkin;  //Default or White
    public InputField customSkin;
    public Image color;
    public InputField metallic;
    public InputField specularity;
    public InputField specularTint;
    public InputField roughness;
    public InputField normalStrength;
    public Toggle specularHighlights;
    public Toggle glossyReflections;

    [Header("Preview")]
    public Image customTexture;


    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
    }

    public void UpdateSosigTexture()
    {
        ManagerUI.instance.sosigMaterial.SetTexture("_MainTex", customTexture.sprite.texture);
        ManagerUI.instance.sosigMaterial.SetTexture("_EmissionMap", customTexture.sprite.texture);

        //Just loop
        for (int i = 0; i < DataLoader.loadedTextures.Count; i++)
        {
            if (DataLoader.loadedTextures[i].name == customTexture.sprite.name + "_Normal")
            {
                ManagerUI.instance.sosigMaterial.SetTexture("_BumpMap", DataLoader.loadedTextures[i]);
            }
        }

    }

    public void OpenCustomSosig(Custom_Sosig template)
    {
        //Clear old buttons

        customSosig = template;

        nameField.SetTextWithoutNotify(template.name);
        SosigEnemyTemplateUI.instance.customSosigTitleText.text = "CUSTOM SOSIG: " + template.name;
        baseSosigID.SetTextWithoutNotify(template.baseSosigID.ToString());

        //Voice
        voiceSet.SetTextWithoutNotify(template.voiceSet.ToString());
        voiceVolume.SetTextWithoutNotify(template.voiceVolume.ToString());
        voicePitch.SetTextWithoutNotify(template.voicePitch.ToString());

        //Scale
        scaleBody[0].SetTextWithoutNotify(template.scaleBody.x.ToString());
        scaleBody[1].SetTextWithoutNotify(template.scaleBody.y.ToString());
        scaleHead[0].SetTextWithoutNotify(template.scaleHead.x.ToString());
        scaleHead[1].SetTextWithoutNotify(template.scaleHead.y.ToString());
        scaleTorso[0].SetTextWithoutNotify(template.scaleTorso.x.ToString());
        scaleTorso[1].SetTextWithoutNotify(template.scaleTorso.y.ToString());
        scaleLegsUpper[0].SetTextWithoutNotify(template.scaleLegsUpper.x.ToString());
        scaleLegsUpper[1].SetTextWithoutNotify(template.scaleLegsUpper.y.ToString());
        scaleLegsLower[0].SetTextWithoutNotify(template.scaleLegsLower.x.ToString());
        scaleLegsLower[1].SetTextWithoutNotify(template.scaleLegsLower.y.ToString());

        //Materials
        useCustomSkin.SetValueWithoutNotify(template.useCustomSkin);
        customSkin.SetTextWithoutNotify(template.customSkin);
        color.color = template.color;
        metallic.SetTextWithoutNotify(template.metallic.ToString());
        specularity.SetTextWithoutNotify(template.specularity.ToString());
        specularTint.SetTextWithoutNotify(template.specularTint.ToString());
        roughness.SetTextWithoutNotify(template.roughness.ToString()) ;
        normalStrength.SetTextWithoutNotify(template.normalStrength.ToString());
        specularHighlights.SetIsOnWithoutNotify(template.specularHighlights);
        glossyReflections.SetIsOnWithoutNotify(template.glossyReflections);

        //Update Preview
        Material sosigMaterial = ManagerUI.instance.sosigMaterial;
        sosigMaterial.SetColor("_Color", template.color);
        sosigMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, template.color, 0.75f));
        //sosigMaterial.SetColor("_SpecColor", Color.Lerp(Color.black, customSosig.color, customSosig.specularTint));
        //sosigMaterial.SetFloat("_GlossMapScale", customSosig.specularity);
        sosigMaterial.SetFloat("_BumpScale", template.normalStrength);
        sosigMaterial.SetFloat("_SpecularHighlights", template.specularHighlights ? 1 : 0);
        sosigMaterial.SetFloat("_GlossyReflections", template.glossyReflections ? 1 : 0);

    }

    public void SaveCustomSosig()
    {
        
        customSosig.name = nameField.text;
        customSosig.baseSosigID = int.Parse(baseSosigID.text);

        //Voice
        customSosig.voiceSet = voiceSet.text;
        customSosig.voiceVolume = float.Parse(voiceVolume.text);
        customSosig.voicePitch = float.Parse(voicePitch.text);

        //Scale
        float minScale = 0.5f;
        float maxScale = 1.5f;
        customSosig.scaleBody 
            = new Vector3(Mathf.Clamp(float.Parse(scaleBody[0].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleBody[1].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleBody[0].text), minScale, maxScale));
        customSosig.scaleHead 
            = new Vector3(Mathf.Clamp(float.Parse(scaleHead[0].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleHead[1].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleHead[0].text), minScale, maxScale));
        customSosig.scaleTorso 
            = new Vector3(Mathf.Clamp(float.Parse(scaleTorso[0].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleTorso[1].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleTorso[0].text), minScale, maxScale));
        customSosig.scaleLegsUpper 
            = new Vector3(Mathf.Clamp(float.Parse(scaleLegsUpper[0].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleLegsUpper[1].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleLegsUpper[0].text), minScale, maxScale));
        customSosig.scaleLegsLower 
            = new Vector3(Mathf.Clamp(float.Parse(scaleLegsLower[0].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleLegsLower[1].text), minScale, maxScale), 
            Mathf.Clamp(float.Parse(scaleLegsLower[0].text), minScale, maxScale));

        //Materials
        customSosig.useCustomSkin = useCustomSkin.value;
        customSosig.customSkin =  customSkin.text;
        customSosig.color = color.color;
        customSosig.metallic = float.Parse(metallic.text);
        customSosig.specularity = float.Parse(specularity.text);
        customSosig.specularTint = Mathf.Clamp01(float.Parse(specularTint.text));
        customSosig.roughness = float.Parse(roughness.text);
        customSosig.normalStrength = float.Parse(normalStrength.text);
        customSosig.specularHighlights = specularHighlights.isOn;
        customSosig.glossyReflections = glossyReflections.isOn;

        //Update Preview
        Material sosigMaterial = ManagerUI.instance.sosigMaterial;
        sosigMaterial.SetColor("_Color", customSosig.color);
        sosigMaterial.SetColor("_EmissionColor", Color.Lerp(Color.black, customSosig.color, 0.75f));
        //sosigMaterial.SetColor("_SpecColor", Color.Lerp(Color.black, customSosig.color, customSosig.specularTint));
        //sosigMaterial.SetFloat("_GlossMapScale", customSosig.specularity);
        sosigMaterial.SetFloat("_BumpScale", customSosig.normalStrength);
        sosigMaterial.SetFloat("_SpecularHighlights", customSosig.specularHighlights ? 1 : 0);
        sosigMaterial.SetFloat("_GlossyReflections", customSosig.glossyReflections ? 1 : 0);

        //Save Log
        ManagerUI.Log("Custom Sosig applied at: " + System.DateTime.Now);

        //Update UI
        SosigEnemyTemplateUI.instance.CustomSosigLoad();
    }
}
