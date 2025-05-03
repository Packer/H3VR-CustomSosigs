using FistVR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CustomSosigLoader;

public class CSL_Vehicle : MonoBehaviour
{
    //Build new Navmesh for this vehicle

    public SosigWearable wearable;

    [Header("Spawning")]
    //public bool requiresSky = false;
    //public float spawnHeightLimit = 4f;
    public Transform vehicleCenter;

    [Header("Wheels")]
    public float wheelRadius = 0.5f;
    public LayerMask wheelCollision;
    public bool wheelsSpin = true;
    public WheelType[] wheels;

    [System.Serializable]
    public class WheelType()
    {
        public Transform wheel;
        public WheelEnum type;
        [Tooltip("How much the wheel can visually turn")]
        public float maxTurnAngle = 40f;
        [Tooltip("Is this wheel included in the vehicle's surface angle calculation (Should only include corner wheels)")]
        public bool surfaceAngleCalculation = false;

        public enum WheelEnum
        {
            Straight,
            Turns,
            InverseTurns,
        }

        [HideInInspector]
        public Vector3 wheelRotation = Vector3.zero;
        [HideInInspector]
        public Vector3 startLocalPosition = Vector3.zero;
    }

    [Header("Agent")]
    [Tooltip("The size of the vehicle, radius should match the width of the vehicle (How narrow of an area it can drive through)")]
    public float agentRadius = 3f;
    [Tooltip("How tall the vehicle is")]
    public float agentHeight = 3;
    [Tooltip("Rate the vehicle gets up to speed")]
    public float agentAccerlation = 2f;
    [Tooltip("How far it stops from its move waypoint, should be minimum Agent Radius")]
    public float agentStoppingDistance = 5f;

    [Header("Movement")]
    //[Tooltip("How fast the vehicle moves")]
    //public float moveSpeed = 10f;
    [Tooltip("Speed the vehicle turns at minimum")]
    public float turnSpeedMin = 1f;
    [Tooltip("Speed the vehicle turns moving at max speed")]
    public float turnSpeedMax = 45f;
    [Tooltip("How fast the vehicle aligns to the surface angle")]
    public float angleTurnRate = 15f;
    protected Transform muzzle;

    [Header("Turret")]
    [Tooltip("Head of the turret")]
    public Transform turretBase;
    [Tooltip("The barrels pivot point")]
    public Transform turretBarrel;
    public Transform turretMuzzle;
    [Tooltip("Degrees per second the turret turns to aim at its target")]
    public float aimSpeed = 45f;
    public bool hideSosigWeapon = true;
    public bool vehicleAlignsToSurface = true;
    //public bool useSosigNavAgent = false;

    [Header("Sosig")]
    public Transform sosigIconsPoint;
    public Transform sosigVision;
    //public SosigWeapon dummyWeapon;
    [HideInInspector] public Sosig sosig;
    protected Vector2 sosigLocalVelocity;
    protected Vector2 sosigLocalVelocityNormal;
    protected Vector3 localTargetPos;

    [Header("Visuals")]
    public Animator bodyAnimator;
    public Animator turretAnimator;

    [Tooltip("Alive Objects get disabled when killed")]
    public GameObject[] aliveObjects;
    [Tooltip("Death Objects get enabled when killed")]
    public GameObject[] deathObjects;
    [Tooltip("Damaged Objects get enabled when mustard reaches amount percentage")]
    public GameObject[] damagedObjects;
    [Range(0, 1f), Tooltip("Percentage of health remaining when the damaged objects get enabled")]
    public float damagedMustardAmount = 0.5f;

    protected float maxMustard = 1000;


    protected float healthDebug = 0;
    bool isDead = false;


    public void Start()
    {
        sosig = Global.GetSosig(transform);
        if(wearable == null)
            wearable = GetComponent<SosigWearable>();

        //Disable incase of error
        if (sosig == null)
        {
            enabled = false;
            CustomSosigLoaderPlugin.Logger.LogError("Vehicle Script - Missing Sosig is the CSL Vehicle component setup correctly? " + name);
            return;
        }

        //Wheel Setup
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].startLocalPosition = wheels[i].wheel.localPosition;
        }

        GM.CurrentSceneSettings.SosigKillEvent += VehicleKillEvent;

        //Update our scale AFTER everything is setup
        Invoke(nameof(LateSosigSetup), 0.5f);
    }

    void Update()
    {
        SosigUpdate();
    }

    void FixedUpdate()
    {
        if (sosig == null || sosig.Agent.enabled == false || isDead)
            return;

        /*
        if (healthDebug != sosig.Mustard)
        {
            healthDebug = sosig.Mustard;
            //Debug.Log(sosig.name + " Health: " + healthDebug);
        }
        */

        sosigLocalVelocity = SosigLocalVelocity();
        sosigLocalVelocityNormal = SosigClampedLocalVelocity();


        if (vehicleAlignsToSurface)
            SurfaceAngle();

        UpdateVisuals();
        WeaponUpdate();
    }

    void LateSosigSetup()
    {
        for (int i = 0; i < sosig.Links.Count; i++)
        {
            //Force links already severed stop them 'breaking'
            sosig.Links[i].m_isJointSevered = true;
        }

        sosig.E.transform.SetParent(vehicleCenter, false);

        //Set new Radius
        sosig.Agent.radius = agentRadius;
        sosig.Agent.height = agentHeight;
        sosig.Agent.acceleration = agentAccerlation;
        sosig.Agent.stoppingDistance = agentStoppingDistance; //Stop vehicle away from player

        //Attach to Agent
        transform.SetParent(sosig.Agent.transform);

        //Move to Correct position
        transform.position = sosig.Agent.transform.position;
        transform.rotation = sosig.Agent.transform.rotation;

        //Muzzle setup
        muzzle = new GameObject().transform;
        muzzle.SetParent(turretMuzzle);
        muzzle.SetPositionAndRotation(turretMuzzle.position, turretMuzzle.rotation);

        // Setup Sosig Hand
        for (int i = 0; i < sosig.Hands.Count; i++)
        {
            sosig.Hands[i].Root = muzzle;
            sosig.Hands[i].Target = muzzle;
            sosig.Hands[i].Point_Aimed = muzzle;
            sosig.Hands[i].Point_AtRest = muzzle;
            sosig.Hands[i].Point_HipFire = muzzle;
            sosig.Hands[i].Point_ShieldHold = muzzle;

            if (sosig.Hands[i].HeldObject != null)
                sosig.Hands[i].HeldObject.O.RootRigidbody.isKinematic = true;
        }

        for (int i = 0; i < sosig.Inventory.Slots.Count; i++)
        {
            sosig.Inventory.Slots[i].Target = muzzle;
        }

        sosig.CoreTarget.position = turretMuzzle.position;
        sosig.Pose_Standing = turretMuzzle;
        sosig.Pose_Crouching = turretMuzzle;
        sosig.Pose_Prone = turretMuzzle;
        sosig.m_targetPose = turretMuzzle;

        sosig.Agent.autoTraverseOffMeshLink = false;

        //Stop weapon from being dropped
        sosig.DoesDropWeaponsOnBallistic = false;

        maxMustard = sosig.Mustard;

        //VISUALS

        for (int i = 0; i < aliveObjects.Length; i++)
        {
            if(aliveObjects[i] != null)
                aliveObjects[i].SetActive(true);
        }

        for (int i = 0; i < deathObjects.Length; i++)
        {
            if(deathObjects[i] != null)
                deathObjects[i].SetActive(false);
        }

        for (int i = 0; i < damagedObjects.Length; i++)
        {
            if(damagedObjects[i] != null)
                damagedObjects[i].SetActive(false);
        }

        //Move icons to above vehicle
        if (sosig.HeadIcons != null)
        {
            for (int i = 0; i < sosig.HeadIcons.Count; i++)
            {
                if (sosig.HeadIcons[i] != null)
                {
                    sosig.HeadIcons[i].transform.SetParent(sosigIconsPoint);
                    sosig.HeadIcons[i].transform.position = sosigIconsPoint.position;
                    sosig.HeadIcons[i].transform.localScale = sosigIconsPoint.localScale * 2;
                }
            }
        }

        //Hide held weapons
        HideSosigWeapon();

        //Disable all Sosig visuals
        for (int i = 0; i < sosig.Meshes.Length; i++)
        {
            sosig.Meshes[i].GetComponent<MeshRenderer>().enabled = false;
        }

        for (int i = 0; i < sosig.Links.Count; i++)
        {
            sosig.Links[i].transform.localScale = Vector3.one * 0.01f;
        }
    }

    void OnDisable()
    {
        GM.CurrentSceneSettings.SosigKillEvent -= VehicleKillEvent;
    }

    protected virtual void VehicleKillEvent(Sosig s)
    {
        if (s == sosig)
        {
            //Already dead
            if (isDead || sosig == null)
                return;

            //Stop it from breaking??
            if (sosig.m_linksDestroyed != null)
            {
                for (int i = 0; i < sosig.m_linksDestroyed.Length; i++)
                {
                    sosig.m_linksDestroyed[i] = true;
                }
            }


            for (int i = 0; i < damagedObjects.Length; i++)
            {
                if(damagedObjects[i] != null)
                    damagedObjects[i].SetActive(false);
            }

            for (int i = 0; i < aliveObjects.Length; i++)
            {
                if(aliveObjects[i] != null)
                    aliveObjects[i].SetActive(false);
            }

            for (int i = 0; i < deathObjects.Length; i++)
            {
                if(deathObjects[i] != null)
                    deathObjects[i].SetActive(true);
            }

            isDead = true;
        }
    }

    void SosigUpdate()
    {
        if(turretMuzzle != null && muzzle != null)
            muzzle.SetPositionAndRotation(turretMuzzle.position, turretMuzzle.rotation);

        if (sosig.CoreRB != null && sosigVision != null)
        {
            sosig.CoreRB.MovePosition(sosigVision.position);
            sosig.CoreRB.transform.position = (sosigVision.position);
            sosig.CoreRB.MoveRotation(sosigVision.rotation);
            sosig.CoreRB.transform.rotation = (sosigVision.rotation);
        }

        if (sosig.Hands != null)
        {
            for (int i = 0; i < sosig.Hands.Count; i++)
            {
                if (sosig.Hands[i] != null && sosig.Hands[i].HeldObject != null)
                {
                    //sosig.Hands[i].Target = sosigHandTarget;
                    sosig.Hands[i].HeldObject.O.RootRigidbody.position = turretMuzzle.position;
                    sosig.Hands[i].HeldObject.O.RootRigidbody.rotation = turretMuzzle.rotation;
                    //sosig.Hands[i].HeldObject.O.transform.LookAt(sosig.Priority.GetTargetPoint());


                    for (int x = 0; x < sosig.Hands[i].vertOffsets.Count; x++)
                    {
                        sosig.Hands[i].vertOffsets[i] = 0;
                    }
                    for (int x = 0; x < sosig.Hands[i].forwardOffsets.Count; x++)
                    {
                        sosig.Hands[i].forwardOffsets[i] = 0;
                    }
                    for (int x = 0; x < sosig.Hands[i].tiltLerpOffsets.Count; x++)
                    {
                        sosig.Hands[i].tiltLerpOffsets[i] = 0;
                    }
                }
            }
        }
    }


    void WeaponUpdate()
    {
        //Relative position to vehicle's body
        localTargetPos = transform.InverseTransformPoint(sosig.Priority.GetTargetPoint());

        switch (sosig.Hands[0].Pose)
        {
            case SosigHand.SosigHandPose.AtRest:
            default:
                //Keep old Aim
                /*
                //We are not aiming
                turretBase.rotation = Quaternion.Lerp(turretBase.localRotation, sosig.Agent.transform.rotation, Time.deltaTime * aimSpeed);
                turretBarrel.rotation = Quaternion.Lerp(turretBarrel.localRotation, sosig.Agent.transform.rotation, Time.deltaTime * aimSpeed * 0.5f);
                */

                break;
            case SosigHand.SosigHandPose.HipFire:
            case SosigHand.SosigHandPose.Aimed:
            case SosigHand.SosigHandPose.Melee:
            case SosigHand.SosigHandPose.ShieldHold:

                Vector3 flatLocalTarget = new Vector3(localTargetPos.x, 0, localTargetPos.z);
                if (flatLocalTarget.sqrMagnitude > 0.001f)
                {
                    Quaternion desiredHeadRotation = Quaternion.LookRotation(flatLocalTarget, Vector3.up);
                    turretBase.localRotation = Quaternion.Lerp(turretBase.localRotation, desiredHeadRotation, Time.deltaTime * aimSpeed);
                }

                //Aim Barrel
                Vector3 lookDirection = sosig.Priority.GetTargetPoint() - turretBarrel.position;
                turretBarrel.rotation = Global.SmoothRotateTowards(lookDirection, turretBarrel.rotation, aimSpeed * 0.5f);
                break;
        }
    }

    void UpdateVisuals()
    {
        //Body Rotation
        if (sosig.Agent.velocity != Vector3.zero)
        {
            float turnRate = Mathf.Lerp(turnSpeedMin, turnSpeedMax, Mathf.InverseLerp(0, SosigMoveSpeed(), sosig.Agent.velocity.magnitude));
            transform.rotation = Global.SmoothRotateTowards(sosig.Agent.velocity, transform.rotation, turnRate);
        }

        //Wheels
        if (wheelsSpin)
        {
            for (int i = 0; i < wheels.Length; i++)
            {

                //wheels[i].wheelRotation.y = Mathf.MoveTowards(wheels[i].wheelRotation.y, sosigLocalVelocityNormal.x > 0 ? angleTurnRate : -angleTurnRate, sosigLocalVelocityNormal.x);


                Vector3 currentTurn = wheels[i].wheel.localRotation.eulerAngles;
                switch (wheels[i].type)
                {
                    case WheelType.WheelEnum.Turns:
                        currentTurn.y = wheels[i].maxTurnAngle * sosigLocalVelocityNormal.x;
                        break;
                    case WheelType.WheelEnum.InverseTurns:
                        currentTurn.y = wheels[i].maxTurnAngle * -sosigLocalVelocityNormal.x;
                        break;
                    default:
                    case WheelType.WheelEnum.Straight:
                        break;
                }

                if (wheelsSpin)
                {
                    currentTurn.x += (sosigLocalVelocity.y / wheelRadius) * Mathf.Rad2Deg;
                }


                wheels[i].wheel.localRotation = Quaternion.Euler(currentTurn);

                //Suspension visuals
                if (Physics.Raycast(wheels[i].wheel.position + (Vector3.up * (wheelRadius / 2)), Vector3.down, out RaycastHit hit, wheelRadius / 2, wheelCollision))
                {
                    Vector3 localHit = wheels[i].wheel.InverseTransformPoint(hit.point); //  wheels[i].wheel.worldToLocalMatrix * hit.point;

                    wheels[i].wheel.position = Vector3.MoveTowards(wheels[i].wheel.position, localHit + Vector3.up * wheelRadius / 2, Time.deltaTime);
                }
                else
                {
                    wheels[i].wheel.localPosition = Vector3.MoveTowards(wheels[i].wheel.localPosition, wheels[i].startLocalPosition, Time.deltaTime);
                }
            }
        }

        //Animators
        if (bodyAnimator != null)
        {
            bodyAnimator.SetFloat("MovementX", sosigLocalVelocityNormal.x);
            bodyAnimator.SetFloat("MovementY", sosigLocalVelocityNormal.y);
        }

        if (turretAnimator != null)
        {

        }

        //Show damaged
        if (isDead == false)
        {
            //If 40 or Below
            if (sosig.Links[0].GetDamageStateIndex() >= 1
                || sosig.Links[0].GetDamageStateIndex() >= 1
                || sosig.Links[0].GetDamageStateIndex() >= 1
                || sosig.Links[0].GetDamageStateIndex() >= 1)
            {
                for (int i = 0; i < damagedObjects.Length; i++)
                {
                    damagedObjects[i].SetActive(true);
                }
            }
        }
    }

    void SurfaceAngle()
    {
        Vector3 normalSum = Vector3.zero;
        int hitCount = 0;

        for (int i = 0; i < wheels.Length; i++)
        {
            if (!wheels[i].surfaceAngleCalculation)
                continue;

            if (Physics.Raycast(wheels[i].wheel.position, -transform.up, out RaycastHit hit, wheelRadius, wheelCollision))
            {
                //Debug.DrawLine(rayPoint.position, hit.point, Color.red);
                normalSum += hit.normal;
                hitCount++;
            }
        }

        if (Physics.Raycast(sosig.Agent.transform.position, -transform.up, out RaycastHit centerHit, wheelRadius, wheelCollision))
        {
            //Debug.DrawLine(sosig.Agent.transform.position, centerHit.point, Color.green);
            normalSum += centerHit.normal;
            hitCount++;
        }

        if (hitCount > 0)
        {
            Vector3 averageNormal = normalSum / hitCount;
            //float slopeAngle = Vector3.Angle(Vector3.up, averageNormal);
            //Debug.Log($"Slope Angle: {slopeAngle:F2}°");

            Quaternion desiredRotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, angleTurnRate * Time.deltaTime);
        }
    }

    Vector2 SosigLocalVelocity()
    {
        Vector3 relativeVelocity = sosig.Agent.transform.InverseTransformDirection(sosig.Agent.velocity);
        Vector2 local2D = new Vector2(relativeVelocity.x, relativeVelocity.z);

        return local2D;
    }

    Vector2 SosigClampedLocalVelocity()
    {
        return Vector2.ClampMagnitude(SosigLocalVelocity() / SosigMoveSpeed(), 1f);
    }

    float SosigMoveSpeed()
    {
        switch (sosig.MoveSpeed)
        {
            default:
            case Sosig.SosigMoveSpeed.Walking:
            case Sosig.SosigMoveSpeed.Still:
                return sosig.Speed_Walk;
            case Sosig.SosigMoveSpeed.Crawling:
                return sosig.Speed_Crawl;
            case Sosig.SosigMoveSpeed.Sneaking:
                return sosig.Speed_Sneak;
            case Sosig.SosigMoveSpeed.Running:
                return sosig.Speed_Run;
        }
    }

    void HideSosigWeapon()
    {
        for (int i = 0; i < sosig.Hands.Count; i++)
        {
            if (sosig.Hands[i].HeldObject == null)
                continue;

            MeshRenderer[] meshComponents = sosig.Hands[i].HeldObject.transform.GetComponentsInChildren<MeshRenderer>();

            for (int x = 0; x < meshComponents.Length; x++)
            {
                meshComponents[x].enabled = false;
            }
        }

    }


    void OnDrawGizmos()
    {

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] != null && wheels[i].wheel != null)
            {
                Gizmos.DrawLine(wheels[i].wheel.position + transform.up * wheelRadius, wheels[i].wheel.position + -transform.up * wheelRadius);
                //DrawCircleGizmo(wheels[i].wheel.position, transform.up, wheelRadius, 8);

                //Gizmos.DrawLine(wheels[i].wheel.position, wheels[i].wheel.position + (Vector3.up * wheelSize));
            }
        }
        Gizmos.color = Color.cyan;
        DrawCircleGizmo(transform.position, transform.forward, agentRadius);
        DrawCircleGizmo(transform.position + transform.up * agentHeight, transform.forward, agentRadius);
    }

    void DrawCircleGizmo(Vector3 center, Vector3 normal, float radius, int segments = 16)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 prevPoint = center + rotation * (Vector3.right * radius);

        for (int i = 1; i <= segments; i++)
        {
            float angle = (i / (float)segments) * Mathf.PI * 2f;
            Vector3 newPoint = center + rotation * (new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius);
            Gizmos.DrawLine(prevPoint, newPoint);
            prevPoint = newPoint;
        }
    }
}
