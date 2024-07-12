using UnityEngine;

[System.Serializable]
public class Custom_Sosig
{
    public string name = "Custom Sosig"; //For internal use
    public int baseSosigID = 0;
    //public string customTextureName = "";

    //Voice
    public float voicePitch = 1;
    public float voiceVolume = 1;

    //Scale
    public Vector3 scaleBody = Vector3.one;     //Width and Height (Don't split x and z)
    public Vector3 scaleHead = Vector3.one;
    public Vector3 scaleTorso = Vector3.one;
    public Vector3 scaleLegsUpper = Vector3.one;
    public Vector3 scaleLegsLower = Vector3.one;

    //Materials
    public int useCustomSkin = 0;  //Default, White or Custom
    public string customSkin = "";
    public Color color = Color.white;
    public float metallic = 0;
    public float specularity = 0.3f;
    public float specularTint = 0f;
    public float roughness = 1f;
    public float normalStrength = 1f;
    public bool specularHighlights = true;
    public bool glossyReflections = true;

    public Custom_Sosig Clone()
    {
        Custom_Sosig template = new Custom_Sosig();
        template.name = name; //For internal use
        template.baseSosigID = baseSosigID;

        //Voice
        template.voicePitch = voicePitch;
        template.voiceVolume = voiceVolume;

        //Scale
        template.scaleBody = scaleBody;     //Width and Height (Don't split x and z)
        template.scaleHead = scaleHead;
        template.scaleTorso = scaleTorso;
        template.scaleLegsUpper = scaleLegsUpper;
        template.scaleLegsLower = scaleLegsLower;

        //Materials
        template.useCustomSkin = useCustomSkin;  //Default or White
        template.color = color;
        template.metallic = metallic;
        template.specularity = specularity;
        template.specularTint = specularTint;
        template.roughness = roughness;
        template.normalStrength = normalStrength;
        template.specularHighlights = specularHighlights;
        template.glossyReflections = glossyReflections;

        return template;
    }
}
