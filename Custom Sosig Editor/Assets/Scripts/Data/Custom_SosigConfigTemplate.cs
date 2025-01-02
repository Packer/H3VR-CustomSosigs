using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Custom_SosigConfigTemplate
{
    public string name = "Config Template";

    //[Header("Supply Raid")]
    //public bool forceOverrideAIEntityParams = false;
    //public string[] spawnItemIDOnDestroy;

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
    public List<float> LinkDamageMultipliers = new List<float> { 10, 2, 1.2f, 0.8f };
    public List<float> LinkStaggerMultipliers = new List<float> { 4, 1, 1, 1.5f };
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

    public Custom_SosigConfigTemplate Clone()
    {
        Custom_SosigConfigTemplate template = new Custom_SosigConfigTemplate();
        template.name = name;

        ///AIEntityParams
        template.ViewDistance = ViewDistance;
        template.StateSightRangeMults = StateSightRangeMults;
        template.HearingDistance = HearingDistance;
        template.StateHearingRangeMults = StateHearingRangeMults;
        template.MaxFOV = MaxFOV;
        template.StateFOVMults = StateFOVMults;

        ///Core Identity Params
        template.HasABrain = HasABrain;
        template.RegistersPassiveThreats = RegistersPassiveThreats;
        template.DoesAggroOnFriendlyFire = DoesAggroOnFriendlyFire;
        template.SearchExtentsModifier = SearchExtentsModifier;
        template.DoesDropWeaponsOnBallistic = DoesDropWeaponsOnBallistic;
        template.CanPickup_Ranged = CanPickup_Ranged;
        template.CanPickup_Melee = CanPickup_Melee;
        template.CanPickup_Other = CanPickup_Other;

        ///TargetPrioritySystemParams
        template.TargetCapacity = TargetCapacity;
        template.TargetTrackingTime = TargetTrackingTime;
        template.NoFreshTargetTime = NoFreshTargetTime;
        template.AssaultPointOverridesSkirmishPointWhenFurtherThan = AssaultPointOverridesSkirmishPointWhenFurtherThan;
        template.TimeInSkirmishToAlert = TimeInSkirmishToAlert;

        ///Movement Params
        template.RunSpeed = RunSpeed;
        template.WalkSpeed = WalkSpeed;
        template.SneakSpeed = SneakSpeed;
        template.CrawlSpeed = CrawlSpeed;
        template.TurnSpeed = TurnSpeed;
        template.MaxJointLimit = MaxJointLimit;
        template.MovementRotMagnitude = MovementRotMagnitude;

        ///Damage Params
        template.AppliesDamageResistToIntegrityLoss = AppliesDamageResistToIntegrityLoss;
        template.TotalMustard = TotalMustard;
        template.BleedDamageMult = BleedDamageMult;
        template.BleedRateMultiplier = BleedRateMultiplier;
        template.BleedVFXIntensity = BleedVFXIntensity;
        template.DamMult_Projectile = DamMult_Projectile;
        template.DamMult_Explosive = DamMult_Explosive;
        template.DamMult_Melee = DamMult_Melee;
        template.DamMult_Piercing = DamMult_Piercing;
        template.DamMult_Blunt = DamMult_Blunt;
        template.DamMult_Cutting = DamMult_Cutting;
        template.DamMult_Thermal = DamMult_Thermal;
        template.DamMult_Chilling = DamMult_Chilling;
        template.DamMult_EMP = DamMult_EMP;
        template.LinkDamageMultipliers = LinkDamageMultipliers;
        template.LinkStaggerMultipliers = LinkStaggerMultipliers;
        template.StartingLinkIntegrity = StartingLinkIntegrity;
        template.StartingChanceBrokenJoint = StartingChanceBrokenJoint;

        ///Shudder Params
        template.ShudderThreshold = ShudderThreshold;

        ///Confusion Params
        template.ConfusionThreshold = ConfusionThreshold;
        template.ConfusionMultiplier = ConfusionMultiplier;
        template.ConfusionTimeMax = ConfusionTimeMax;

        ///Stun Params
        template.StunThreshold = StunThreshold;
        template.StunMultiplier = StunMultiplier;
        template.StunTimeMax = StunTimeMax;

        ///Unconsciousness Params
        template.CanBeKnockedOut = CanBeKnockedOut;
        template.MaxUnconsciousTime = MaxUnconsciousTime;

        ///Resistances
        template.CanBeGrabbed = CanBeGrabbed;
        template.CanBeSevered = CanBeSevered;
        template.CanBeStabbed = CanBeStabbed;

        ///Suppression
        template.CanBeSurpressed = CanBeSurpressed;
        template.SuppressionMult = SuppressionMult;

        ///Death Flags
        template.DoesJointBreakKill_Head = DoesJointBreakKill_Head;
        template.DoesJointBreakKill_Upper = DoesJointBreakKill_Upper;
        template.DoesJointBreakKill_Lower = DoesJointBreakKill_Lower;
        template.DoesSeverKill_Head = DoesSeverKill_Head;
        template.DoesSeverKill_Upper = DoesSeverKill_Upper;
        template.DoesSeverKill_Lower = DoesSeverKill_Lower;
        template.DoesExplodeKill_Head = DoesExplodeKill_Head;
        template.DoesExplodeKill_Upper = DoesExplodeKill_Upper;
        template.DoesExplodeKill_Lower = DoesExplodeKill_Lower;

        ///SpawnOnLinkDestroy
        template.UsesLinkSpawns = UsesLinkSpawns;
        template.LinkSpawns = LinkSpawns;
        template.LinkSpawnChance = LinkSpawnChance;

        return template;
    }
}