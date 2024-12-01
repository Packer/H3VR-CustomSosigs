using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace CustomSosigLoader
{
    [System.Serializable]
    public class Custom_SosigConfigTemplate
    {
        public SosigConfigTemplate Initialize()
        {
            SosigConfigTemplate config = ScriptableObject.CreateInstance<SosigConfigTemplate>();

            //AI Entity Params
            config.ViewDistance = ViewDistance;
            config.StateSightRangeMults = StateSightRangeMults;
            config.HearingDistance = HearingDistance;
            config.StateHearingRangeMults = StateHearingRangeMults;
            config.MaxFOV = MaxFOV;
            config.StateFOVMults = StateFOVMults;

            //Core Identity Params
            config.HasABrain = HasABrain;

            config.RegistersPassiveThreats = RegistersPassiveThreats;
            config.DoesAggroOnFriendlyFire = DoesAggroOnFriendlyFire;
            config.SearchExtentsModifier = SearchExtentsModifier;
            config.DoesDropWeaponsOnBallistic = DoesDropWeaponsOnBallistic;
            config.CanPickup_Ranged = CanPickup_Ranged;
            config.CanPickup_Melee = CanPickup_Melee;
            config.CanPickup_Other = CanPickup_Other;

            //TargetPrioritySystemParams
            config.TargetCapacity = TargetCapacity;
            config.TargetTrackingTime = TargetTrackingTime;
            config.NoFreshTargetTime = NoFreshTargetTime;
            config.AssaultPointOverridesSkirmishPointWhenFurtherThan = AssaultPointOverridesSkirmishPointWhenFurtherThan;
            //config.TimeInSkirmishToAlert = TimeInSkirmishToAlert;

            //Movement Params
            config.RunSpeed = RunSpeed;
            config.WalkSpeed = WalkSpeed;
            config.SneakSpeed = SneakSpeed;
            config.CrawlSpeed = CrawlSpeed;
            config.TurnSpeed = TurnSpeed;
            config.MaxJointLimit = MaxJointLimit;
            config.MovementRotMagnitude = MovementRotMagnitude;

            //Damage Params
            config.AppliesDamageResistToIntegrityLoss = AppliesDamageResistToIntegrityLoss;
            config.TotalMustard = TotalMustard;
            config.BleedDamageMult = BleedDamageMult;
            config.BleedRateMultiplier = BleedRateMultiplier;
            config.BleedVFXIntensity = BleedVFXIntensity;
            config.DamMult_Projectile = DamMult_Projectile;
            config.DamMult_Explosive = DamMult_Explosive;
            config.DamMult_Melee = DamMult_Melee;
            config.DamMult_Piercing = DamMult_Piercing;
            config.DamMult_Blunt = DamMult_Blunt;
            config.DamMult_Cutting = DamMult_Cutting;
            config.DamMult_Thermal = DamMult_Thermal;
            config.DamMult_Chilling = DamMult_Chilling;
            config.DamMult_EMP = DamMult_EMP;
            config.LinkDamageMultipliers = LinkDamageMultipliers;
            config.LinkStaggerMultipliers = LinkStaggerMultipliers;
            config.StartingLinkIntegrity = StartingLinkIntegrity;
            config.StartingChanceBrokenJoint = StartingChanceBrokenJoint;

            //Shudder Params
            config.ShudderThreshold = ShudderThreshold;

            //Confusion Params
            config.ConfusionThreshold = ConfusionThreshold;
            config.ConfusionMultiplier = ConfusionMultiplier;
            config.ConfusionTimeMax = ConfusionTimeMax;

            //Stun Params
            config.StunThreshold = StunThreshold;
            config.StunMultiplier = StunMultiplier;
            config.StunTimeMax = StunTimeMax;

            //Unconsciousness Params
            config.CanBeKnockedOut = CanBeKnockedOut;
            config.MaxUnconsciousTime = MaxUnconsciousTime;

            //Resistances
            config.CanBeGrabbed = CanBeGrabbed;
            config.CanBeSevered = CanBeSevered;
            config.CanBeStabbed = CanBeStabbed;

            //Suppression
            config.CanBeSurpressed = CanBeSurpressed;
            config.SuppressionMult = SuppressionMult;

            //Death Flags
            config.DoesJointBreakKill_Head = DoesJointBreakKill_Head;
            config.DoesJointBreakKill_Upper = DoesJointBreakKill_Upper;
            config.DoesJointBreakKill_Lower = DoesJointBreakKill_Lower;
            config.DoesSeverKill_Head = DoesSeverKill_Head;
            config.DoesSeverKill_Upper = DoesSeverKill_Upper;
            config.DoesSeverKill_Lower = DoesSeverKill_Lower;
            config.DoesExplodeKill_Head = DoesExplodeKill_Head;
            config.DoesExplodeKill_Upper = DoesExplodeKill_Upper;
            config.DoesExplodeKill_Lower = DoesExplodeKill_Lower;

            //SpawnOnLinkDestroy
            config.UsesLinkSpawns = UsesLinkSpawns;

            if (config.LinkSpawns == null)
                config.LinkSpawns = new List<FVRObject>();
            Global.ItemIDToList(LinkSpawns.ToArray(), config.LinkSpawns);
            config.LinkSpawnChance = LinkSpawnChance;

            //config.OverrideSpeechSet = new SosigSpeechSet();
            //config.OverrideSpeechSet.BasePitch

            return config;
        }

        public string name;

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
        public List<float> LinkDamageMultipliers;
        public List<float> LinkStaggerMultipliers;
        public List<Vector2> StartingLinkIntegrity;
        public List<float> StartingChanceBrokenJoint;

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
}
