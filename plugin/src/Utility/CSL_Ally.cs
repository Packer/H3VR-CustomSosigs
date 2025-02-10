using UnityEngine;
using UnityEngine.AI;
using FistVR;
using System.Collections.Generic;

namespace CustomSosigLoader
{
    public class CSL_Ally : MonoBehaviour
    {
        [HideInInspector] public Sosig sosig;
        [HideInInspector] public SosigWearable wearable;
        [HideInInspector] public Transform followPlayer;
        private float timeout = 0;
        public float followDistance = 4;
        [HideInInspector] bool followSide = false;  //Left - false, Right = true

        public void Start()
        {
            if (sosig == null)
                GetSosig();

            if (sosig == null)
            {
                Debug.Log("Failed to find sosig body on Ally Wearable");
                enabled = false;
                return;
            }
            SetAlly();

            followSide = Random.Range(0,10) > 5 ? true : false;
        }

        void Update()
        {
            if (sosig == null || timeout > Time.time)
                return;

            //Random update
            timeout = Time.time + Random.Range(0.0f, 5.0f);

            if (!InCombat(sosig.CurrentOrder) &&  Vector3.SqrMagnitude(followPlayer.position) > followDistance)
                SetWaypointToPlayer();
        }

        public static bool InCombat(Sosig.SosigOrder order)
        {
            switch (order)
            {
                default:
                case Sosig.SosigOrder.Wander:
                case Sosig.SosigOrder.Idle:
                case Sosig.SosigOrder.Disabled:
                case Sosig.SosigOrder.GuardPoint:
                case Sosig.SosigOrder.PathTo:
                    return false;
                case Sosig.SosigOrder.Flee:
                case Sosig.SosigOrder.Investigate:
                case Sosig.SosigOrder.StaticShootAt:
                case Sosig.SosigOrder.StaticMeleeAttack:
                case Sosig.SosigOrder.Assault:
                case Sosig.SosigOrder.TakeCover:
                case Sosig.SosigOrder.Skirmish:
                case Sosig.SosigOrder.SearchForEquipment:
                    return true;
            }
        }

        void SetWaypointToPlayer()
        {
            NavMeshHit hit;
            Vector3 playerPosition;

            Vector3 sidePosition = followSide ? followPlayer.forward + followPlayer.right : followPlayer.forward - followPlayer.right;

            if (NavMesh.SamplePosition(followPlayer.position - (sidePosition * followDistance), out hit, followDistance, NavMesh.AllAreas))
                playerPosition = hit.position;
            else
                playerPosition = followPlayer.position;
            List<Vector3> pathPoints = [playerPosition, playerPosition];
            List<Vector3> pathDirs = [followPlayer.rotation.eulerAngles, followPlayer.rotation.eulerAngles];

            sosig.CommandPathTo(
                pathPoints,
                pathDirs,
                1,
                Vector2.one * 4,
                2f,
                Sosig.SosigMoveSpeed.Running,
                Sosig.PathLoopType.LoopEndless,
                null,
                0.2f,
                1f,
                true,
                50f);
        }

        void GetSosig()
        {
            sosig = transform.root.GetComponent<Sosig>();

            if (sosig == null)
            {
                wearable = GetComponent<SosigWearable>();
                if (wearable != null)
                    sosig = wearable.S;
            }

            if (sosig == null)
                sosig = GetComponent<Sosig>();
        }

        void SetAlly()
        {
            sosig.SetIFF(GM.CurrentPlayerBody.GetPlayerIFF());

            if (followPlayer != null)
                return;

            if (CustomSosigLoaderPlugin.h3mpEnabled)
            {
                //TODO Find Nearest Player, for now default local player
                followPlayer = GM.CurrentPlayerBody.Head;
            }
            else
            {
                followPlayer = GM.CurrentPlayerBody.Head;
            }
        }
    }
}