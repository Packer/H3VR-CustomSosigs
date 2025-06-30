using FistVR;
using System.Diagnostics;
using UnityEngine;

namespace CustomSosigLoader;

public class CSL_VehicleDamage : MonoBehaviour, IFVRDamageable
{
    public CSL_Vehicle vehicle;

    [Tooltip("List of blacklisted damage types that will do no damage, e.g. Melee for a tank")]
    public Damage.DamageClass[] damageBlacklist;

    [Tooltip("How much the damage gets multiplied when damaging this wearable passer")]
    public float damage = 1;

    [Tooltip("What kind of effect is applied to the vehicle when this is damaged")]
    public VehicleDamageEffect effect = VehicleDamageEffect.None;
    [Tooltip("How long this effect gets applied for")]
    public float effectTime = 1;

    public void Damage(Damage d)
    {
        float adjustedDamage = damage;
        VehicleDamageEffect adjustedEffect = effect;

        //Blacklisted Damage, deals no damage
        for (int i = 0; i < damageBlacklist.Length; i++)
        {
            if (d.Class == damageBlacklist[i])
            {
                //Stop all damage but still pass damage to alert sosig
                adjustedDamage = 0;
                adjustedEffect = VehicleDamageEffect.None;
                break;
            }
        }

        //Damage our Vehicle and give it a status effect
        vehicle.Damage(adjustedDamage, adjustedEffect, effectTime);
    }

    public void Start()
    {
        //Missing Vehicle
        if (vehicle == null)
        {
            CustomSosigLoaderPlugin.Logger.LogError(name + " is missing a vehicle reference, disabling");
            enabled = false;
        }
    }
}

public enum VehicleDamageEffect
{
    None,
    Stun,
    Confused,
    Freeze,
}