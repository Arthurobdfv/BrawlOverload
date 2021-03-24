using BrawlServer.Common;
using GameClient;
using GameServer;
using GameServer.Common;
using MapGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BrawlServer
{
    class BrawlClientHandler : ClientHandle
    {
        public override Dictionary<int, Client.PacketHandler> PacketHandlers
        {
            get
            {
                return new Dictionary<int, Client.PacketHandler>()
                {
                    {(int)BrawlServerPackets.Welcome, Welcome},
                    {(int)BrawlServerPackets.Connect, SpawnPlayer },
                    {(int)BrawlServerPackets.PlayerPosition, PlayerPosition },
                    {(int)BrawlServerPackets.PlayerRotation, PlayerRotation },
                    {(int)BrawlServerPackets.Disconnect, PlayerDisconnect },
                    {(int)BrawlServerPackets.MapTilesPositions, RecieveMap }
                };
            }
        }

        public void Welcome(Packet packet)
        {
            var msg = packet.ReadString();
            var value = packet.ReadInt();
            Debug.Log($"{msg}");
            Client.instance.myId = value;
            (Client._sender as BrawlClientSender).WelcomeRecieved();

            Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
        }

        public void SpawnPlayer(Packet _packet)
        {
            int id = _packet.ReadInt();
            string userName = _packet.ReadString();
            Vector3 _position = _packet.ReadVector3();
            Quaternion _rotation = _packet.ReadQuaternion();

            GameManager.instance.SpawnPlayer(id, userName, _position, _rotation);
        }


        private void PlayerPosition(Packet packet)
        {
            int id = packet.ReadInt();
            Vector3 pos = packet.ReadVector3();

            GameManager.players[id].transform.position = pos;
        }

        private void PlayerRotation(Packet packet)
        {
            int id = packet.ReadInt();
            Quaternion rot = packet.ReadQuaternion();
            Debug.Log("Recieved rotation " + rot);
            GameManager.players[id].gameObject.transform.rotation = rot;
        }

        private void PlayerDisconnect(Packet packet)
        {
            int id = packet.ReadInt();
            GameManager.Disconnect(id);
        }

        private void RecieveMap(Packet packet)
        {
            var length = packet.ReadInt();
            var Pos = new Vector3[length];
            for(int i=0; i<length; i++)
            {
                Pos[i] = packet.ReadVector3();
            }
            Injector.GetInstance<MapGenerator>()?.GenerateTerrain(Pos);
        }


    }
}
