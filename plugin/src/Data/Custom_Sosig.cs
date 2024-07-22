using UnityEngine;
using FistVR;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_Sosig
    {
        public string name; //For internal use
        public SosigEnemyID baseSosigID = SosigEnemyID.Misc_Dummy;

        //Voice
        public string voiceSet = "";
        public float voicePitch = 1.15f;
        public float voiceVolume = 0.4f;

        //Scale
        public Vector3 scaleBody = Vector3.one;
        public Vector3 scaleHead = Vector3.one;
        public Vector3 scaleTorso = Vector3.one;
        public Vector3 scaleLegsUpper = Vector3.one;
        public Vector3 scaleLegsLower = Vector3.one;

        //Mesh
        public bool hideHeadMesh = false;
        public bool hideTorsoMesh = false;
        public bool hideLegsUpperMesh = false;
        public bool hideLegsLowerMesh = false;

        //Materials
        //public int useCustomSkin = 0;  //Default or White or Custom
        public string customSkin = "";  //Name.png Name_Normal.png Name_MASR.png
        public Color color;
        public float metallic = 0;
        public float specularity = 0.3f;
        public float specularTint = 0f;
        public float roughness = 1f;
        public float normalStrength = 1f;
        public bool specularHighlights = true;
        public bool glossyReflections = true;

        //Internal
        public string directory;
        public Texture2D albedo;
        public Texture2D normalmap;
        public Texture2D masr;
    }
}
