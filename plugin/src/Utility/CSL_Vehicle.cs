using FistVR;
using UnityEngine;

namespace CustomSosigLoader;

public class CSL_Vehicle : MonoBehaviour
{
    [Header("Spawning")]
    public bool requiresSky = false;
    public float spawnHeightLimit = 4f;

    [Header("Wheels")]
    public Transform[] wheelLocatons;
    public float wheelDepth = 0.5f;
    public LayerMask wheelCollision;

    [Header("Agent")]
    public float agentRadius = 1f;
    public float agentHeight = 2;

    [Header("Movement")]
    public float moveSpeed = 10f;
    public float turnSpeed = 45f;

    [Header("Turret")]
    public Transform turret;
    public Transform barrel;

    [HideInInspector] public Sosig sosig;

    [Header("Visuals")]
    [Tooltip("Alive Objects get disabled when killed")]
    public GameObject[] aliveObjects;
    [Tooltip("Death Objects get enabled when killed")]
    public GameObject[] deathObjects;
    [Tooltip("Damaged Objects get enabled when mustard reaches amount percentage")]
    public GameObject[] damagedObjects;
    [MinMaxRange(0, 1f)]
    public float damagedMustardAmount = 0.5f;

    protected float maxMustard = 1000;

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
    }

    void FixedUpdate()
    {
        UpdateVisuals();
        VehicleAngle();
        WeaponUpdate();
    }

    void WeaponUpdate()
    {
        switch (sosig.Hands[0].Pose)
        {
            case SosigHand.SosigHandPose.AtRest:
            default:
                //We are not aiming
                break;
            case SosigHand.SosigHandPose.HipFire:
            case SosigHand.SosigHandPose.Aimed:
            case SosigHand.SosigHandPose.Melee:
            case SosigHand.SosigHandPose.ShieldHold:
                //We are AIMING
                break;
        }



        switch (sosig.Hands[0].HeldObject.UsageState)
        {
            case SosigWeapon.SosigWeaponUsageState.Firing:
                //FIRE OUR PROJECTILE

                break;
            case SosigWeapon.SosigWeaponUsageState.Reloading:
            default:
                //DO NOTHING
                break;
        }
    }

    void UpdateVisuals()
    {
        if (sosig.BodyState == Sosig.SosigBodyState.Dead)
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

    void SetupSosig()
    {
        for (int i = 0; i < sosig.Links.Count; i++)
        {
            //Disable all Sosig Colliders
            sosig.Links[i].C.enabled = false;
        }

        for (int i = 0; i < sosig.Meshes.Length; i++)
        {
            //Disable all Sosig visuals
            sosig.Meshes[i].GetComponent<MeshRenderer>().enabled = false;
        }

        //Set new Radius
        sosig.Agent.radius = agentRadius;
        sosig.Agent.height = agentHeight;

        //Movement
        sosig.Agent.speed = moveSpeed;
        sosig.Agent.angularSpeed = turnSpeed;

        //Attach to Agent
        transform.SetParent(sosig.Agent.transform);

        //Move to Correct position
        transform.position = sosig.Agent.transform.position;
        transform.rotation = sosig.Agent.transform.rotation;

        // Setup Sosig Hand
        sosig.Hands[0].Root = sosig.Hands[1].Root = turret;
        sosig.Hands[0].Target = sosig.Hands[1].Target = barrel;
        sosig.Hands[0].Point_Aimed = sosig.Hands[1].Point_Aimed = barrel;
        sosig.Hands[0].Point_AtRest = sosig.Hands[1].Point_AtRest = barrel;
        sosig.Hands[0].Point_HipFire = sosig.Hands[1].Point_HipFire = barrel;
        sosig.Hands[0].Point_ShieldHold = sosig.Hands[1].Point_ShieldHold = barrel;

        sosig.Priority.GetTargetPoint();


        //AIMTOWARDS = sosig.Hands[0].m_aimTowardPoint

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
    }

    void VehicleWheelVisuals()
    {
        
    }

    void VehicleAngle()
    {
        if (sosig.transform == null)
        {
            return;
        }

        Vector3 normalSum = Vector3.zero;
        int hitCount = 0;

        foreach (Transform rayPoint in wheelLocatons) 
        {
            if (Physics.Raycast(rayPoint.position, -transform.up, out RaycastHit hit, wheelDepth, wheelCollision))
            {
                Debug.DrawLine(rayPoint.position, hit.point, Color.red);
                normalSum += hit.normal;
                hitCount++;
            }
        }

        if (Physics.Raycast(sosig.Agent.transform.position, -transform.up, out RaycastHit centerHit, wheelDepth, wheelCollision))
        {
            Debug.DrawLine(sosig.Agent.transform.position, centerHit.point, Color.green);
            normalSum += centerHit.normal;
            hitCount++;
        }

        if (hitCount > 0)
        {
            Vector3 averageNormal = normalSum / hitCount;
            float slopeAngle = Vector3.Angle(Vector3.up, averageNormal);
            //Debug.Log($"Slope Angle: {slopeAngle:F2}°");

            // Optional: Align object to slope
            transform.rotation = Quaternion.FromToRotation(transform.up, averageNormal) * transform.rotation;
        }
    }

    void OnDrawGizmos()
    {
        DrawCircleGizmo(transform.position, transform.forward, agentRadius, 12);

        for (int i = 0; i < wheelLocatons.Length; i++)
        {
            if (wheelLocatons[i] != null)
                Gizmos.DrawLine(wheelLocatons[i].position, wheelLocatons[i].position + (Vector3.up * wheelDepth));
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
