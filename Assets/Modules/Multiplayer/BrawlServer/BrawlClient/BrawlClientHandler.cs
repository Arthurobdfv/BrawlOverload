using BrawlServer.Common;
using GameClient;
using GameServer;
using GameServer.Common;
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
                    {(int)BrawlServerPackets.Connect, SpawnPlayer }
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


    }
}
