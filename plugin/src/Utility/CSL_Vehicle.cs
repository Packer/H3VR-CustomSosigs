using FistVR;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CustomSosigLoader;

public class CSL_Vehicle : MonoBehaviour
{
    //Build new Navmesh for this vehicle
    //public static Dictionary<string, NavMeshBuildSettings> navMeshSettings = new Dictionary<string, NavMeshBuildSettings>();

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
        public Vector2 wheelRotation = Vector2.zero;
        [HideInInspector]
        public Vector3 startLocalPosition = Vector3.zero;
    }

    [Header("Agent")]
    [Tooltip("The size of the vehicle, radius should match the width of the vehicle (How narrow of an area it can drive through)")]
    public float agentRadius = 1f;
    [Tooltip("How tall the vehicle is")]
    public float agentHeight = 2;
    [Tooltip("Rate the vehicle gets up to speed")]
    public float agentAccerlation = 2.5f;
    [Tooltip("How far it stops from its move waypoint, should be minimum Agent Radius")]
    public float agentStoppingDistance = 1f;

    [Header("Movement")]
    [Tooltip("How fast the vehicle moves")]
    public float moveSpeed = 10f;
    [Tooltip("Speed the vehicle turns at minimum")]
    public float turnSpeedMin = 10f;
    [Tooltip("Speed the vehicle turns moving at max speed")]
    public float turnSpeedMax = 45f;
    [Tooltip("How fast the vehicle aligns to the surface angle")]
    public float angleTurnRate = 15f;

    [Header("Turret")]
    [Tooltip("Head of the turret")]
    public Transform turretBase;
    [Tooltip("The barrels pivot point")]
    public Transform turretBarrel;
    public Transform turretMuzzle;
    [Tooltip("Degrees per second the turret turns to aim at its target")]
    public float aimSpeed = 45f;

    [Header("Sosig")]
    public Transform sosigIconsPoint;
    public Transform sosigWeaponRoot;
    public Transform sosigWeapon;
    public SosigWeapon dummyWeapon;
    [HideInInspector] public Sosig sosig;
    protected Vector2 sosigLocalVelocity;
    protected Vector2 sosigLocalVelocityNormal;

    [Header("Visuals")]
    public Animator bodyAnimator;
    public Animator turretAnimator;

    [Tooltip("Alive Objects get disabled when killed")]
    public GameObject[] aliveObjects;
    [Tooltip("Death Objects get enabled when killed")]
    public GameObject[] deathObjects;
    [Tooltip("Damaged Objects get enabled when mustard reaches amount percentage")]
    public GameObject[] damagedObjects;
    [MinMaxRange(0, 1f), Tooltip("Percentage of health remaining when the damaged objects get enabled")]
    public float damagedMustardAmount = 0.5f;

    protected float maxMustard = 1000;

    public bool vehicleAlignsToSurface = true;


    public void Start()
    {
        sosig = Global.GetSosig(transform);

        //Disable incase of error
        if (sosig == null)
        {
            enabled = false;
            CustomSosigLoaderPlugin.Logger.LogError("Vehicle Script - Missing Sosig is the CSL Vehicle component setup correctly? " + name);
            return;
        }

        SetupSosig();

        //Wheel Setup
        for (int i = 0; i < wheels.Length; i++)
        {
            wheels[i].startLocalPosition = wheels[i].wheel.localPosition;
        }
    }

    void SetupSosig()
    {

        for (int i = 0; i < sosig.Links.Count; i++)
        {
            //Disable all Sosig Colliders
            sosig.Links[i].C.enabled = false;
            sosig.Links[i].R.isKinematic = true;
            sosig.Links[i].R.MovePosition(vehicleCenter.position);
        }

        /*
        for (int i = 0; i < sosig.Meshes.Length; i++)
        {
            //Disable all Sosig visuals
            sosig.Meshes[i].GetComponent<MeshRenderer>().enabled = false;
        }
        */

        //Set new Radius
        sosig.Agent.radius = agentRadius;
        sosig.Agent.height = agentHeight;
        sosig.Agent.acceleration = agentAccerlation;
        sosig.Agent.stoppingDistance = agentStoppingDistance; //Stop vehicle away from player

        //Movement
        sosig.Agent.speed = moveSpeed;
        sosig.Agent.angularSpeed = turnSpeedMax;

        //Attach to Agent
        //transform.SetParent(sosig.Agent.transform);

        //Move to Correct position
        transform.position = sosig.Agent.transform.position;
        transform.rotation = sosig.Agent.transform.rotation;

        // Setup Sosig Hand
        sosig.Hands[0].Root = sosig.Hands[1].Root = sosigWeaponRoot;
        sosig.Hands[0].Target = sosig.Hands[1].Target = sosigWeapon;
        sosig.Hands[0].Point_Aimed = sosig.Hands[1].Point_Aimed = sosigWeapon;
        sosig.Hands[0].Point_AtRest = sosig.Hands[1].Point_AtRest = sosigWeapon;
        sosig.Hands[0].Point_HipFire = sosig.Hands[1].Point_HipFire = sosigWeapon;
        sosig.Hands[0].Point_ShieldHold = sosig.Hands[1].Point_ShieldHold = sosigWeapon;

        //Move icons to above vehicle
        for (int i = 0; i < sosig.HeadIcons.Count; i++)
        {
            sosig.HeadIcons[i].transform.position = sosigIconsPoint.position;
        }


        maxMustard = sosig.Mustard;

        //VISUALS

        for (int i = 0; i < aliveObjects.Length; i++)
        {
            aliveObjects[i].SetActive(true);
        }

        for (int i = 0; i < deathObjects.Length; i++)
        {
            deathObjects[i].SetActive(false);
        }

        sosig.Inventory.Slots[0].HeldObject = dummyWeapon;
    }


    void FixedUpdate()
    {
        if (sosig == null || sosig.Agent.enabled == false)
            return;

        sosigLocalVelocity = SosigLocalVelocity();
        sosigLocalVelocityNormal = SosigClampedLocalVelocity();

        UpdateVisuals();
        WeaponUpdate();

        if(vehicleAlignsToSurface)
            SurfaceAngle();
    }

    void WeaponUpdate()
    {


        switch (sosig.Hands[0].Pose)
        {
            case SosigHand.SosigHandPose.AtRest:
            default:
                //We are not aiming
                turretBase.rotation = Global.SmoothRotateTowards(sosig.Agent.transform.forward, turretBase.rotation, aimSpeed);
                turretBarrel.rotation = Global.SmoothRotateTowards(sosig.Agent.transform.forward, turretBarrel.rotation, aimSpeed * 0.5f); // Half speed for barrel
                break;
            case SosigHand.SosigHandPose.HipFire:
            case SosigHand.SosigHandPose.Aimed:
            case SosigHand.SosigHandPose.Melee:
            case SosigHand.SosigHandPose.ShieldHold:

                //Aim Base
                Vector3 lookLevel = sosig.Priority.GetTargetPoint();
                lookLevel.y = turretBase.position.y;
                Vector3 lookDirection = lookLevel - turretBase.position;
                turretBase.rotation = Global.SmoothRotateTowards(lookDirection, turretBase.rotation, aimSpeed);

                //Aim Barrel
                lookDirection = sosig.Priority.GetTargetPoint() - turretBarrel.position;
                turretBarrel.rotation = Global.SmoothRotateTowards(lookDirection, turretBarrel.rotation, aimSpeed);

                /*
                turretBase.LookAt(targetPos);
                Vector3 lookEuler = turretBase.localRotation.eulerAngles;
                Vector3 baseRot = lookEuler;
                baseRot.x = 0;
                baseRot.z = 0;
                turretBase.localRotation = Quaternion.Euler(baseRot);

                Vector3 barrelRot = lookEuler;
                turretBarrel.LookAt(targetPos);
                */
                break;
        }


        if (sosig.Hands[0].HeldObject == null)
            return;

        switch (sosig.Hands[0].HeldObject.UsageState)
        {
            case SosigWeapon.SosigWeaponUsageState.Firing:
                //FIRE OUR PROJECTILE
                //Play fire animation

                break;
            case SosigWeapon.SosigWeaponUsageState.Reloading:
            default:
                //DO NOTHING

                break;
        }
    }

    void UpdateVisuals()
    {
        //Body Rotation
        if (sosig.Agent.velocity != Vector3.zero)
        {
            float turnRate = Mathf.Lerp(turnSpeedMin, turnSpeedMax, Mathf.InverseLerp(0, moveSpeed, sosig.Agent.velocity.magnitude));
            transform.rotation = Global.SmoothRotateTowards(sosig.Agent.velocity, transform.rotation, turnRate);
        }

        //Wheels
        if (wheelsSpin)
        {
            for (int i = 0; i < wheels.Length; i++)
            {

                if (wheelsSpin)
                {
                    wheels[i].wheelRotation.x += (sosigLocalVelocity.y / wheelRadius) * Mathf.Rad2Deg;
                }

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

                wheels[i].wheel.localRotation = Quaternion.Euler(currentTurn);

                //Suspension visuals
                if (Physics.Raycast(wheels[i].wheel.position, Vector3.down * wheelRadius, out RaycastHit hit, wheelRadius, wheelCollision))
                {
                    wheels[i].wheel.position = hit.point + Vector3.up * wheelRadius;
                }
                else
                {
                    wheels[i].wheel.localPosition = wheels[i].startLocalPosition;
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



        //Game Objects
        if (sosig.BodyState == Sosig.SosigBodyState.Dead || sosig.Mustard <= 0 || sosig.m_diedFromType != default)
        {
            for (int i = 0; i < damagedObjects.Length; i++)
            {
                damagedObjects[i].SetActive(false);
            }

            for (int i = 0; i < aliveObjects.Length; i++)
            {
                aliveObjects[i].SetActive(false);
            }

            for (int i = 0; i < deathObjects.Length; i++)
            {
                deathObjects[i].SetActive(true);
            }
        }
        else
        {
            //Show damaged
            if (Mathf.InverseLerp(0, maxMustard, sosig.Mustard) <= damagedMustardAmount)
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
        return Vector2.ClampMagnitude(SosigLocalVelocity() / moveSpeed, 1f);
    }


    void SetupNavMesh()
    {
        //Build Navmesh for vehicles here if there isn't one in this scene
        /*
        sosig.Agent.agentTypeID = 1;
        NavMesh.GetSettingsByIndex(1);
        NavMesh.Get
        NavMeshBuildSettings ahhh = NavMesh.CreateSettings();
        ahhh.agent
        */
    }

    void OnDrawGizmos()
    {
        DrawCircleGizmo(transform.position, transform.forward, agentRadius, 12);

        for (int i = 0; i < wheels.Length; i++)
        {
            if (wheels[i] != null && wheels[i].wheel != null)
            {
                Gizmos.DrawLine(wheels[i].wheel.position + transform.up * wheelRadius, wheels[i].wheel.position + -transform.up * wheelRadius);
                //DrawCircleGizmo(wheels[i].wheel.position, transform.up, wheelRadius, 8);

                //Gizmos.DrawLine(wheels[i].wheel.position, wheels[i].wheel.position + (Vector3.up * wheelSize));
            }
        }
    }

    void DrawCircleGizmo(Vector3 center, Vector3 normal, float radius, int segments = 32)
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
