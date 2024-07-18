using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using H3MP;
using H3MP.Tracking;
using H3MP.Networking;
using FistVR;

namespace CustomSosigLoader
{
    public class SosigMP : MonoBehaviour
    {
        public static SosigMP instance;

        private int sosigUpdate_ID = -1;

        void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            if (CustomSosigLoaderPlugin.h3mpEnabled)
                StartNetworking();
        }

        //--------------------------------------------------------------------
        // NETWORKING
        //--------------------------------------------------------------------

        void StartNetworking()
        {
            if (Networking.ServerRunning())
            {
                if (Networking.IsHost() || Client.isFullyConnected)
                {
                    SetupPacketTypes();
                }
            }
        }

        void SetupPacketTypes()
        {
            if (Networking.IsHost())
            {
                if (Mod.registeredCustomPacketIDs.ContainsKey("CSL_Update"))
                    sosigUpdate_ID = Mod.registeredCustomPacketIDs["CSL_Update"];
                else
                    sosigUpdate_ID = Server.RegisterCustomPacketType("CSL_Update");
                Mod.customPacketHandlers[sosigUpdate_ID] = CustomSosig_Handler;
            }
            else //Client
            {
                if (Mod.registeredCustomPacketIDs.ContainsKey("CSL_Update"))
                {
                    sosigUpdate_ID = Mod.registeredCustomPacketIDs["CSL_Update"];
                    Mod.customPacketHandlers[sosigUpdate_ID] = CustomSosig_Handler;
                }
                else
                {
                    ClientSend.RegisterCustomPacketType("CSL_Update");
                    Mod.CustomPacketHandlerReceived += CustomSosig_Received;
                }
            }
        }

        public void CustomSosig_Send(Sosig sosig, int sosigID)
        {
            TrackedSosig tracker = sosig.gameObject.GetComponent<TrackedSosig>();

            if (!Networking.ServerRunning() || Networking.IsClient() || tracker)
                return;

            Packet packet = new Packet(sosigUpdate_ID);
                        packet.Write(tracker.sosigData.trackedID);
            packet.Write(sosigID);
            ServerSend.SendTCPDataToAll(packet, true);

            Debug.Log("Server - Sending Custom sosig data to " + tracker.data.trackedID + " ID: " + sosigID);
        }

        void CustomSosig_Handler(int clientID, Packet packet)
        {
            int trackedID = packet.ReadInt();
            int sosigID = packet.ReadInt();

            Debug.Log("Client - Sosig data: " + trackedID + " ID " + sosigID);

            if (Client.objects.Length > trackedID && Client.objects[trackedID] != null)
            {
                Sosig sosig = Client.objects[trackedID].physical.gameObject.GetComponent<Sosig>();
                CustomSosigLoaderPlugin.customSosigIDs.TryGetValue((SosigEnemyID)sosigID, out SosigConfigTemplate template);
                Hooks.SetupCustomSosig(sosig, template);
            }
        }


        // CLIENT RECEIEVD

        void CustomSosig_Received(string identifier, int index)
        {
            if (identifier == "CSL_Update")
            {
                sosigUpdate_ID = index;
                Mod.customPacketHandlers[index] = CustomSosig_Handler;
                Mod.CustomPacketHandlerReceived -= CustomSosig_Received;
            }
        }
    }
}
