using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Custom_SosigConfigTemplate
{
    [Header("Supply Raid")]
    public bool forceOverrideAIEntityParams = false;
    public string[] spawnItemIDOnDestroy;

    [Header("AIEntityParams")]
    public float ViewDistance = 250f;
    public Vector3 StateSightRangeMults = new Vector3(0.1f, 0.35f, 1);
    public float HearingDistance = 300f;
    public Vector3 StateHearingRangeMults = new Vector3(0.6f, 1, 1);
    public float MaxFOV = 105f;
    public Vector3 StateFOVMults = new Vector3(0.5f, 0.6f, 1);

    [Header("Core Identity Params")]
    public bool HasABrain = true;
    public bool RegistersPassiveThreats = false;
    public bool DoesAggroOnFriendlyFire = false;
    public float SearchExtentsModifier = 1;
    public bool DoesDropWeaponsOnBallistic = true;
    public bool CanPickup_Ranged = true;
    public bool CanPickup_Melee = true;
    public bool CanPickup_Other = true;

    [Header("TargetPrioritySystemParams")]
    public int TargetCapacity = 5;
    public float TargetTrackingTime = 2;
    public float NoFreshTargetTime = 1.5f;
    public float AssaultPointOverridesSkirmishPointWhenFurtherThan = 200;
    public float TimeInSkirmishToAlert = 1f;

    [Header("Movement Params")]
    public float RunSpeed = 3.5f;
    public float WalkSpeed = 1.4f;
    public float SneakSpeed = 0.6f;
    public float CrawlSpeed = 0.3f;
    public float TurnSpeed = 2f;
    public float MaxJointLimit = 6f;
    public float MovementRotMagnitude = 10f;

    [Header("Damage Params")]
    public bool AppliesDamageResistToIntegrityLoss = false;
    public float TotalMustard = 100f;
    public float BleedDamageMult = 0.5f;
    public float BleedRateMultiplier = 1f;
    public float BleedVFXIntensity = 0.2f;
    public float DamMult_Projectile = 1;
    public float DamMult_Explosive = 1;
    public float DamMult_Melee = 1;
    public float DamMult_Piercing = 1;
    public float DamMult_Blunt = 1;
    public float DamMult_Cutting = 1;
    public float DamMult_Thermal = 1;
    public float DamMult_Chilling = 1;
    public float DamMult_EMP = 1;
    public List<float> LinkDamageMultipliers = new List<float> { 4, 2, 1.2f, 1 };
    public List<float> LinkStaggerMultipliers = new List<float> { 8, 0.3f, 0.8f, 1 };
    public List<Vector2> StartingLinkIntegrity = new List<Vector2> { new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100), new Vector2(100, 100) };
    public List<float> StartingChanceBrokenJoint = new List<float> { 0, 0, 0, 0 };

    [Header("Shudder Params")]
    public float ShudderThreshold = 2f;

    [Header("Confusion Params")]
    public float ConfusionThreshold = 0.3f;
    public float ConfusionMultiplier = 6f;
    public float ConfusionTimeMax = 4f;

    [Header("Stun Params")]
    public float StunThreshold = 1.4f;
    public float StunMultiplier = 2;
    public float StunTimeMax = 4f;

    [Header("Unconsciousness Params")]
    public bool CanBeKnockedOut = true;
    public float MaxUnconsciousTime = 90f;

    [Header("Resistances")]
    public bool CanBeGrabbed = true;
    public bool CanBeSevered = true;
    public bool CanBeStabbed = true;

    [Header("Suppression")]
    public bool CanBeSurpressed = true;
    public float SuppressionMult = 1;

    [Header("Death Flags")]
    public bool DoesJointBreakKill_Head = true;
    public bool DoesJointBreakKill_Upper = false;
    public bool DoesJointBreakKill_Lower = false;
    public bool DoesSeverKill_Head = true;
    public bool DoesSeverKill_Upper = true;
    public bool DoesSeverKill_Lower = true;
    public bool DoesExplodeKill_Head = true;
    public bool DoesExplodeKill_Upper = true;
    public bool DoesExplodeKill_Lower = true;

    [Header("SpawnOnLinkDestroy")]
    public bool UsesLinkSpawns = false;
    public List<string> LinkSpawns;
    public List<float> LinkSpawnChance;
}