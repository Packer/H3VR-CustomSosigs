using FistVR;
using System.Diagnostics;
using UnityEngine;

namespace CustomSosigLoader;

public class CSL_WearablePasser : SosigWearblePasser, IFVRDamageable
{
    [Tooltip("List of blacklisted damage types that will do no damage, e.g. Melee for a tank")]
    public Damage.DamageClass[] damageBlacklist;
    [Tooltip("How much the damage gets multiplied when damaging this wearable passer")]
    public float damageMultiplier = 1;
    [Tooltip("What body part of the sosig will receive the damage, Wearable Link is Default")]
    public LinkEnum damageLink = LinkEnum.WearableLink;

    public enum LinkEnum
    {
        Head,
        Torso,
        UpperLegs,
        LowerLegs,
        WearableLink = 4,
    }

    public new void Damage(Damage d)
    {
        //Null damage if blacklist but still do damage event to alert sosig
        for (int i = 0; i < damageBlacklist.Length; i++)
        {
            if (d.Class == damageBlacklist[i])
            {
                //Stop all damage but still pass damage to alert sosig
                d.Dam_Blinding = 0;
                d.Dam_Cutting = 0;
                d.Dam_Chilling = 0;
                d.Dam_EMP = 0;
                d.Dam_Piercing = 0;
                d.Dam_Stunning = 0;
                d.Dam_Thermal = 0;
                d.Dam_TotalKinetic = 0;
                d.Dam_TotalEnergetic = 0;
                break;
            }
        }
        d.Dam_Blinding  *= damageMultiplier;
        d.Dam_Cutting   *= damageMultiplier;
        d.Dam_Chilling  *= damageMultiplier;
        d.Dam_EMP       *= damageMultiplier;
        d.Dam_Piercing  *= damageMultiplier;
        d.Dam_Stunning  *= damageMultiplier;
        d.Dam_Thermal   *= damageMultiplier;
        d.Dam_TotalKinetic *= damageMultiplier;
        d.Dam_TotalEnergetic *= damageMultiplier;

        switch (damageLink)
        {
            case LinkEnum.Head:
                if (W.S.Links[0] != null && !W.S.Links[0].IsExploded)
                    W.S.Links[0].Damage(d);
                break;
            case LinkEnum.Torso:
                if (W.S.Links[1] != null && !W.S.Links[1].IsExploded)
                    W.S.Links[1].Damage(d);
                break;
            case LinkEnum.UpperLegs:
                if (W.S.Links[2] != null && !W.S.Links[2].IsExploded)
                    W.S.Links[2].Damage(d);
                break;
            case LinkEnum.LowerLegs:
                if(W.S.Links[3] != null && !W.S.Links[3].IsExploded)
                    W.S.Links[3].Damage(d);
                break;
            case LinkEnum.WearableLink:
            default:
                W.Damage(d);
                break;
        }
    }
}