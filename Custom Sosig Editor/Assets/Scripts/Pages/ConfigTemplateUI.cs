using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ConfigTemplateUI : MonoBehaviour
{
    public static ConfigTemplateUI instance;

    public Custom_SosigConfigTemplate configTemplate;

    #region Inputs Vars
    public InputField nameField;

    [Header("AIEntityParams")]
    public InputField ViewDistance;
    public InputField[] StateSightRangeMults = new InputField[3];
    public InputField HearingDistance;
    public InputField[] StateHearingRangeMults = new InputField[3];
    public InputField MaxFOV;
    public InputField[] StateFOVMults = new InputField[3];

    [Header("Core Identity Params")]
    public Toggle HasABrain;
    public Toggle RegistersPassiveThreats;
    public Toggle DoesAggroOnFriendlyFire;
    public InputField SearchExtentsModifier;
    public Toggle DoesDropWeaponsOnBallistic;
    public Toggle CanPickup_Ranged;
    public Toggle CanPickup_Melee;
    public Toggle CanPickup_Other;

    [Header("TargetPrioritySystemParams")]
    public InputField TargetCapacity;
    public InputField TargetTrackingTime;
    public InputField NoFreshTargetTime;
    public InputField AssaultPointOverridesSkirmishPointWhenFurtherThan;
    public InputField TimeInSkirmishToAlert;

    [Header("Movement Params")]
    public InputField RunSpeed;
    public InputField WalkSpeed;
    public InputField SneakSpeed;
    public InputField CrawlSpeed;
    public InputField TurnSpeed;
    public InputField MaxJointLimit;
    public InputField MovementRotMagnitude;

    [Header("Damage Params")]
    public Toggle AppliesDamageResistToIntegrityLoss;
    public InputField TotalMustard;
    public InputField BleedDamageMult;
    public InputField BleedRateMultiplier;
    public InputField BleedVFXIntensity;
    public InputField DamMult_Projectile;
    public InputField DamMult_Explosive;
    public InputField DamMult_Melee;
    public InputField DamMult_Piercing;
    public InputField DamMult_Blunt;
    public InputField DamMult_Cutting;
    public InputField DamMult_Thermal;
    public InputField DamMult_Chilling;
    public InputField DamMult_EMP;
    public InputField[] LinkDamageMultipliers = new InputField[4];
    public InputField[] LinkStaggerMultipliers = new InputField[4];
    public InputField[] StartingLinkIntegrityX = new InputField[4];
    public InputField[] StartingLinkIntegrityY = new InputField[4];
    public InputField[] StartingChanceBrokenJoint = new InputField[4];

    [Header("Shudder Params")]
    public InputField ShudderThreshold;

    [Header("Confusion Params")]
    public InputField ConfusionThreshold;
    public InputField ConfusionMultiplier;
    public InputField ConfusionTimeMax;

    [Header("Stun Params")]
    public InputField StunThreshold;
    public InputField StunMultiplier;
    public InputField StunTimeMax;

    [Header("Unconsciousness Params")]
    public Toggle CanBeKnockedOut;
    public InputField MaxUnconsciousTime;

    [Header("Resistances")]
    public Toggle CanBeGrabbed;
    public Toggle CanBeSevered;
    public Toggle CanBeStabbed;

    [Header("Suppression")]
    public Toggle CanBeSurpressed;
    public InputField SuppressionMult;

    [Header("Death Flags")]
    public Toggle DoesJointBreakKill_Head;
    public Toggle DoesJointBreakKill_Upper;
    public Toggle DoesJointBreakKill_Lower;
    public Toggle DoesSeverKill_Head;
    public Toggle DoesSeverKill_Upper;
    public Toggle DoesSeverKill_Lower;
    public Toggle DoesExplodeKill_Head;
    public Toggle DoesExplodeKill_Upper;
    public Toggle DoesExplodeKill_Lower;

    [Header("SpawnOnLinkDestroy")]
    public Toggle UsesLinkSpawns;
    public List<GenericButton> LinkSpawns = new List<GenericButton>();
    public List<GenericButton> LinkSpawnChance = new List<GenericButton>();
    [SerializeField] GameObject linkSpawnPrefab;
    [SerializeField] Transform linkSpawnContent;
    #endregion


    private void Awake()
    {
        instance = this;
    }

    public void OpenConfigTemplate(Custom_SosigConfigTemplate template)
    {
        configTemplate = template;

        nameField.SetTextWithoutNotify(template.name);
        SosigEnemyTemplateUI.instance.configTemplateTitleText.text = "CONFIG TEMPLATE: " + template.name;

        //AIEntityParams
        ViewDistance.SetTextWithoutNotify(template.ViewDistance.ToString());
        StateSightRangeMults[0].SetTextWithoutNotify(template.StateSightRangeMults[0].ToString());
        StateSightRangeMults[1].SetTextWithoutNotify(template.StateSightRangeMults[1].ToString());
        StateSightRangeMults[2].SetTextWithoutNotify(template.StateSightRangeMults[2].ToString());
        HearingDistance.SetTextWithoutNotify(template.HearingDistance.ToString());
        StateHearingRangeMults[0].SetTextWithoutNotify(template.StateHearingRangeMults[0].ToString());
        StateHearingRangeMults[1].SetTextWithoutNotify(template.StateHearingRangeMults[1].ToString());
        StateHearingRangeMults[2].SetTextWithoutNotify(template.StateHearingRangeMults[2].ToString());
        MaxFOV.SetTextWithoutNotify(template.MaxFOV.ToString());
        StateFOVMults[0].SetTextWithoutNotify(template.StateFOVMults[0].ToString());
        StateFOVMults[1].SetTextWithoutNotify(template.StateFOVMults[1].ToString());
        StateFOVMults[2].SetTextWithoutNotify(template.StateFOVMults[2].ToString());

        //Core Identity Params
        HasABrain.SetIsOnWithoutNotify(template.HasABrain);
        RegistersPassiveThreats.SetIsOnWithoutNotify(template.RegistersPassiveThreats);
        DoesAggroOnFriendlyFire.SetIsOnWithoutNotify(template.DoesAggroOnFriendlyFire);
        SearchExtentsModifier.SetTextWithoutNotify(template.SearchExtentsModifier.ToString());
        DoesDropWeaponsOnBallistic.SetIsOnWithoutNotify(template.DoesDropWeaponsOnBallistic);
        CanPickup_Ranged.SetIsOnWithoutNotify(template.CanPickup_Ranged);
        CanPickup_Melee.SetIsOnWithoutNotify(template.CanPickup_Melee);
        CanPickup_Other.SetIsOnWithoutNotify(template.CanPickup_Other);

        //TargetPrioritySystemParams
        TargetCapacity.SetTextWithoutNotify(template.TargetCapacity.ToString());
        TargetTrackingTime.SetTextWithoutNotify(template.TargetTrackingTime.ToString());
        NoFreshTargetTime.SetTextWithoutNotify(template.NoFreshTargetTime.ToString());
        AssaultPointOverridesSkirmishPointWhenFurtherThan.SetTextWithoutNotify(template.AssaultPointOverridesSkirmishPointWhenFurtherThan.ToString());
        TimeInSkirmishToAlert.SetTextWithoutNotify(template.TimeInSkirmishToAlert.ToString());

        //Movement Params
        RunSpeed.SetTextWithoutNotify(template.RunSpeed.ToString());
        WalkSpeed.SetTextWithoutNotify(template.WalkSpeed.ToString());
        SneakSpeed.SetTextWithoutNotify(template.SneakSpeed.ToString());
        CrawlSpeed.SetTextWithoutNotify(template.CrawlSpeed.ToString());
        TurnSpeed.SetTextWithoutNotify(template.TurnSpeed.ToString());
        MaxJointLimit.SetTextWithoutNotify(template.MaxJointLimit.ToString());
        MovementRotMagnitude.SetTextWithoutNotify(template.MovementRotMagnitude.ToString());

        //Damage Params
        AppliesDamageResistToIntegrityLoss.SetIsOnWithoutNotify(template.AppliesDamageResistToIntegrityLoss);
        TotalMustard.SetTextWithoutNotify(template.TotalMustard.ToString());
        BleedDamageMult.SetTextWithoutNotify(template.BleedDamageMult.ToString());
        BleedRateMultiplier.SetTextWithoutNotify(template.BleedRateMultiplier.ToString());
        BleedVFXIntensity.SetTextWithoutNotify(template.BleedVFXIntensity.ToString());
        DamMult_Projectile.SetTextWithoutNotify(template.DamMult_Projectile.ToString());
        DamMult_Explosive.SetTextWithoutNotify(template.DamMult_Explosive.ToString());
        DamMult_Melee.SetTextWithoutNotify(template.DamMult_Melee.ToString());
        DamMult_Piercing.SetTextWithoutNotify(template.DamMult_Piercing.ToString());
        DamMult_Blunt.SetTextWithoutNotify(template.DamMult_Blunt.ToString());
        DamMult_Cutting.SetTextWithoutNotify(template.DamMult_Cutting.ToString());
        DamMult_Thermal.SetTextWithoutNotify(template.DamMult_Thermal.ToString());
        DamMult_Chilling.SetTextWithoutNotify(template.DamMult_Chilling.ToString());
        DamMult_EMP.SetTextWithoutNotify(template.DamMult_EMP.ToString());

        LinkDamageMultipliers[0].SetTextWithoutNotify(template.LinkDamageMultipliers[0].ToString());
        LinkDamageMultipliers[1].SetTextWithoutNotify(template.LinkDamageMultipliers[1].ToString());
        LinkDamageMultipliers[2].SetTextWithoutNotify(template.LinkDamageMultipliers[2].ToString());
        LinkDamageMultipliers[3].SetTextWithoutNotify(template.LinkDamageMultipliers[3].ToString());

        LinkStaggerMultipliers[0].SetTextWithoutNotify(template.LinkStaggerMultipliers[0].ToString());
        LinkStaggerMultipliers[1].SetTextWithoutNotify(template.LinkStaggerMultipliers[1].ToString());
        LinkStaggerMultipliers[2].SetTextWithoutNotify(template.LinkStaggerMultipliers[2].ToString());
        LinkStaggerMultipliers[3].SetTextWithoutNotify(template.LinkStaggerMultipliers[3].ToString());

        StartingLinkIntegrityX[0].SetTextWithoutNotify(template.StartingLinkIntegrity[0].x.ToString());
        StartingLinkIntegrityX[1].SetTextWithoutNotify(template.StartingLinkIntegrity[1].x.ToString());
        StartingLinkIntegrityX[2].SetTextWithoutNotify(template.StartingLinkIntegrity[2].x.ToString());
        StartingLinkIntegrityX[3].SetTextWithoutNotify(template.StartingLinkIntegrity[3].x.ToString());

        StartingLinkIntegrityY[0].SetTextWithoutNotify(template.StartingLinkIntegrity[0].y.ToString());
        StartingLinkIntegrityY[1].SetTextWithoutNotify(template.StartingLinkIntegrity[1].y.ToString());
        StartingLinkIntegrityY[2].SetTextWithoutNotify(template.StartingLinkIntegrity[2].y.ToString());
        StartingLinkIntegrityY[3].SetTextWithoutNotify(template.StartingLinkIntegrity[3].y.ToString());

        StartingChanceBrokenJoint[0].SetTextWithoutNotify(template.StartingChanceBrokenJoint[0].ToString());
        StartingChanceBrokenJoint[1].SetTextWithoutNotify(template.StartingChanceBrokenJoint[1].ToString());
        StartingChanceBrokenJoint[2].SetTextWithoutNotify(template.StartingChanceBrokenJoint[2].ToString());
        StartingChanceBrokenJoint[3].SetTextWithoutNotify(template.StartingChanceBrokenJoint[3].ToString());

        //Shudder Params
        ShudderThreshold.SetTextWithoutNotify(template.ShudderThreshold.ToString());

        //Confusion Params
        ConfusionThreshold.SetTextWithoutNotify(template.ConfusionThreshold.ToString());
        ConfusionMultiplier.SetTextWithoutNotify(template.ConfusionMultiplier.ToString());
        ConfusionTimeMax.SetTextWithoutNotify(template.ConfusionTimeMax.ToString());

        //Stun Params
        StunThreshold.SetTextWithoutNotify(template.StunThreshold.ToString());
        StunMultiplier.SetTextWithoutNotify(template.StunMultiplier.ToString());
        StunTimeMax.SetTextWithoutNotify(template.StunTimeMax.ToString());

        //Unconsciousness Params
        CanBeKnockedOut.SetIsOnWithoutNotify(template.CanBeKnockedOut);
        MaxUnconsciousTime.SetTextWithoutNotify(template.MaxUnconsciousTime.ToString());

        //Resistances
        CanBeGrabbed.SetIsOnWithoutNotify(template.CanBeGrabbed);
        CanBeSevered.SetIsOnWithoutNotify(template.CanBeSevered);
        CanBeStabbed.SetIsOnWithoutNotify(template.CanBeStabbed);

        //Suppression
        CanBeSurpressed.SetIsOnWithoutNotify(template.CanBeSurpressed);
        SuppressionMult.SetTextWithoutNotify(template.SuppressionMult.ToString());

        //Death Flags
        DoesJointBreakKill_Head.SetIsOnWithoutNotify(template.DoesJointBreakKill_Head);
        DoesJointBreakKill_Upper.SetIsOnWithoutNotify(template.DoesJointBreakKill_Upper);
        DoesJointBreakKill_Lower.SetIsOnWithoutNotify(template.DoesJointBreakKill_Lower);
        DoesSeverKill_Head.SetIsOnWithoutNotify(template.DoesSeverKill_Head);
        DoesSeverKill_Upper.SetIsOnWithoutNotify(template.DoesSeverKill_Upper);
        DoesSeverKill_Lower.SetIsOnWithoutNotify(template.DoesSeverKill_Lower);
        DoesExplodeKill_Head.SetIsOnWithoutNotify(template.DoesExplodeKill_Head);
        DoesExplodeKill_Upper.SetIsOnWithoutNotify(template.DoesExplodeKill_Upper);
        DoesExplodeKill_Lower.SetIsOnWithoutNotify(template.DoesExplodeKill_Lower);

        //SpawnOnLinkDestroy
        UsesLinkSpawns.SetIsOnWithoutNotify(template.UsesLinkSpawns);
        SetupLinkSpawnCollection();
    }

    public void AddLinkSpawn()
    {
        CreateLinkSpawnButton();
    }

    public void RemoveLinkSpawn(GenericButton button)
    {
        LinkSpawns.Remove(button);
        LinkSpawnChance.Remove(button);

        Destroy(button.gameObject);

        SaveConfigTemplate();
    }

    GenericButton CreateLinkSpawnButton(float chance = 1, string id = "")
    {
        GenericButton btn = Instantiate(linkSpawnPrefab, linkSpawnContent).GetComponent<GenericButton>();
        btn.inputField.SetTextWithoutNotify(id);
        btn.inputFieldX.SetTextWithoutNotify(chance.ToString());

        btn.gameObject.SetActive(true);
        LinkSpawns.Add(btn);
        LinkSpawnChance.Add(btn);
        return btn;
    }

    void SetupLinkSpawnCollection()
    {
        //Clear oldones
        for (int i = LinkSpawns.Count - 1; i >= 0; i--)
        {
            if (LinkSpawns[i])
                Destroy(LinkSpawns[i].gameObject);
        }
        LinkSpawns.Clear();
        LinkSpawnChance.Clear();

        //Generate buttons
        if(configTemplate.LinkSpawns == null)
            configTemplate.LinkSpawns = new List<string>();
        if(configTemplate.LinkSpawnChance == null)
            configTemplate.LinkSpawnChance = new List<float>();
        int count = configTemplate.LinkSpawns.Count > configTemplate.LinkSpawnChance.Count 
                  ? configTemplate.LinkSpawns.Count : configTemplate.LinkSpawnChance.Count;

        for (int i = 0; i < count; i++)
        {
            CreateLinkSpawnButton(configTemplate.LinkSpawnChance[i], configTemplate.LinkSpawns[i]);
        }
    }

    public void SaveConfigTemplate()
    {
        configTemplate.name = nameField.text;

        //AIEntityParams
        configTemplate.ViewDistance = float.Parse(ViewDistance.text);
        configTemplate.StateSightRangeMults.x = float.Parse(StateSightRangeMults[0].text);
        configTemplate.StateSightRangeMults.y = float.Parse(StateSightRangeMults[1].text);
        configTemplate.StateSightRangeMults.z = float.Parse(StateSightRangeMults[2].text);

        configTemplate.HearingDistance = float.Parse(HearingDistance.text);
        configTemplate.StateHearingRangeMults.x = float.Parse(StateHearingRangeMults[0].text);
        configTemplate.StateHearingRangeMults.y = float.Parse(StateHearingRangeMults[1].text);
        configTemplate.StateHearingRangeMults.z = float.Parse(StateHearingRangeMults[2].text);


        configTemplate.MaxFOV = float.Parse(MaxFOV.text);
        configTemplate.StateFOVMults.x = float.Parse(StateFOVMults[0].text);
        configTemplate.StateFOVMults.y = float.Parse(StateFOVMults[1].text);
        configTemplate.StateFOVMults.z = float.Parse(StateFOVMults[2].text);

        //Core Identity Params
        configTemplate.HasABrain = HasABrain.isOn;
        configTemplate.RegistersPassiveThreats = RegistersPassiveThreats.isOn;
        configTemplate.DoesAggroOnFriendlyFire = DoesAggroOnFriendlyFire.isOn;
        configTemplate.SearchExtentsModifier = float.Parse(SearchExtentsModifier.text);
        configTemplate.DoesDropWeaponsOnBallistic = DoesDropWeaponsOnBallistic.isOn;
        configTemplate.CanPickup_Ranged = CanPickup_Ranged.isOn;
        configTemplate.CanPickup_Melee =  CanPickup_Melee.isOn;
        configTemplate.CanPickup_Other =  CanPickup_Other.isOn;

        //TargetPrioritySystemParams                                                  float.Parse(//TargetPrioritySystemParams
        configTemplate.TargetCapacity = int.Parse(TargetCapacity.text);
        configTemplate.TargetTrackingTime = float.Parse(TargetTrackingTime.text);
        configTemplate.NoFreshTargetTime = float.Parse(NoFreshTargetTime.text);
        configTemplate.AssaultPointOverridesSkirmishPointWhenFurtherThan = float.Parse(AssaultPointOverridesSkirmishPointWhenFurtherThan.text);
        configTemplate.TimeInSkirmishToAlert = float.Parse(TimeInSkirmishToAlert.text);

        //Movement Params
        configTemplate.RunSpeed =             float.Parse(RunSpeed.text);
        configTemplate.WalkSpeed =            float.Parse(WalkSpeed.text);
        configTemplate.SneakSpeed =           float.Parse(SneakSpeed.text);
        configTemplate.CrawlSpeed =           float.Parse(CrawlSpeed.text);
        configTemplate.TurnSpeed =            float.Parse(TurnSpeed.text);
        configTemplate.MaxJointLimit =        float.Parse(MaxJointLimit.text);
        configTemplate.MovementRotMagnitude = float.Parse(MovementRotMagnitude.text);

        //Damage Params
        configTemplate.AppliesDamageResistToIntegrityLoss = AppliesDamageResistToIntegrityLoss.isOn;
        configTemplate.TotalMustard =        float.Parse(TotalMustard.text);
        configTemplate.BleedDamageMult =     float.Parse(BleedDamageMult.text);
        configTemplate.BleedRateMultiplier = float.Parse(BleedRateMultiplier.text);
        configTemplate.BleedVFXIntensity =   float.Parse(BleedVFXIntensity.text);
        configTemplate.DamMult_Projectile =  float.Parse(DamMult_Projectile.text);
        configTemplate.DamMult_Explosive =   float.Parse(DamMult_Explosive.text);
        configTemplate.DamMult_Melee =       float.Parse(DamMult_Melee.text);
        configTemplate.DamMult_Piercing =    float.Parse(DamMult_Piercing.text);
        configTemplate.DamMult_Blunt =       float.Parse(DamMult_Blunt.text);
        configTemplate.DamMult_Cutting =     float.Parse(DamMult_Cutting.text);
        configTemplate.DamMult_Thermal =     float.Parse(DamMult_Thermal.text);
        configTemplate.DamMult_Chilling =    float.Parse(DamMult_Chilling.text);
        configTemplate.DamMult_EMP =         float.Parse(DamMult_EMP.text);
        configTemplate.LinkDamageMultipliers[0] = float.Parse(LinkDamageMultipliers[0].text);
        configTemplate.LinkDamageMultipliers[1] = float.Parse(LinkDamageMultipliers[1].text);
        configTemplate.LinkDamageMultipliers[2] = float.Parse(LinkDamageMultipliers[2].text);
        configTemplate.LinkDamageMultipliers[3] = float.Parse(LinkDamageMultipliers[3].text);

        configTemplate.LinkStaggerMultipliers[0] = float.Parse(LinkStaggerMultipliers[0].text);
        configTemplate.LinkStaggerMultipliers[1] = float.Parse(LinkStaggerMultipliers[1].text);
        configTemplate.LinkStaggerMultipliers[2] = float.Parse(LinkStaggerMultipliers[2].text);
        configTemplate.LinkStaggerMultipliers[3] = float.Parse(LinkStaggerMultipliers[3].text);

        Vector2 linkX = new Vector2(float.Parse(StartingLinkIntegrityX[0].text), float.Parse(StartingLinkIntegrityY[0].text));
        Vector2 linkY = new Vector2(float.Parse(StartingLinkIntegrityX[1].text), float.Parse(StartingLinkIntegrityY[1].text));
        Vector2 linkZ = new Vector2(float.Parse(StartingLinkIntegrityX[2].text), float.Parse(StartingLinkIntegrityY[2].text));
        Vector2 linkW = new Vector2(float.Parse(StartingLinkIntegrityX[3].text), float.Parse(StartingLinkIntegrityY[3].text));
        configTemplate.StartingLinkIntegrity = new List<Vector2> { linkX, linkY, linkZ, linkW };

        configTemplate.StartingChanceBrokenJoint[0] = Mathf.Clamp01(float.Parse(StartingChanceBrokenJoint[0].text));
        configTemplate.StartingChanceBrokenJoint[1] = Mathf.Clamp01(float.Parse(StartingChanceBrokenJoint[1].text));
        configTemplate.StartingChanceBrokenJoint[2] = Mathf.Clamp01(float.Parse(StartingChanceBrokenJoint[2].text));
        configTemplate.StartingChanceBrokenJoint[3] = Mathf.Clamp01(float.Parse(StartingChanceBrokenJoint[3].text));

        //Shudder Params
        configTemplate.ShudderThreshold =    float.Parse(ShudderThreshold.text);
        //Confusion Params                   
        configTemplate.ConfusionThreshold =  float.Parse(ConfusionThreshold.text);
        configTemplate.ConfusionMultiplier = float.Parse(ConfusionMultiplier.text);
        configTemplate.ConfusionTimeMax =    float.Parse(ConfusionTimeMax.text);
                                             
        //Stun Params                        
        configTemplate.StunThreshold =       float.Parse(StunThreshold.text);
        configTemplate.StunMultiplier =      float.Parse(StunMultiplier.text);
        configTemplate.StunTimeMax =         float.Parse(StunTimeMax.text);
                                             
        //Unconsciousness Params             
        configTemplate.CanBeKnockedOut =     CanBeKnockedOut.isOn;
        configTemplate.MaxUnconsciousTime =  float.Parse(MaxUnconsciousTime.text);

        //Resistances 
        configTemplate.CanBeGrabbed = CanBeGrabbed.isOn;
        configTemplate.CanBeSevered = CanBeSevered.isOn;
        configTemplate.CanBeStabbed = CanBeStabbed.isOn;

        //Suppression
        configTemplate.CanBeSurpressed = CanBeSurpressed.isOn;
        configTemplate.SuppressionMult = float.Parse(SuppressionMult.text);

        //Death Flags
        configTemplate.DoesJointBreakKill_Head =  DoesJointBreakKill_Head.isOn;
        configTemplate.DoesJointBreakKill_Upper = DoesJointBreakKill_Upper.isOn;
        configTemplate.DoesJointBreakKill_Lower = DoesJointBreakKill_Lower.isOn;
        configTemplate.DoesSeverKill_Head = DoesSeverKill_Head.isOn;
        configTemplate.DoesSeverKill_Upper =      DoesSeverKill_Upper.isOn;
        configTemplate.DoesSeverKill_Lower =      DoesSeverKill_Lower.isOn;
        configTemplate.DoesExplodeKill_Head =     DoesExplodeKill_Head.isOn;
        configTemplate.DoesExplodeKill_Upper =    DoesExplodeKill_Upper.isOn;
        configTemplate.DoesExplodeKill_Lower =    DoesExplodeKill_Lower.isOn;

        //SpawnOnLinkDestroy
        configTemplate.UsesLinkSpawns = UsesLinkSpawns.isOn;
        Debug.Log(LinkSpawns.Count);
        configTemplate.LinkSpawns = Global.GenericButtonsToStringList(LinkSpawns.ToArray());
        configTemplate.LinkSpawnChance = Global.GenericButtonsXToFloatList(LinkSpawnChance.ToArray());

        //Save Log
        ManagerUI.Log("Config Template applied at: " + System.DateTime.Now);

        //Last
        SosigEnemyTemplateUI.instance.ConfigTemplateLoad();
    }


}
