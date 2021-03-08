using GameServer;
using GameServer.Common;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using static GameClient.Client;

namespace GameClient
{

    public abstract class ClientHandle : IClientHandle
    {
        public abstract Dictionary<int, Client.PacketHandler> PacketHandlers { get; }

        public void Handle(int _id, Packet _p)
        {
            PacketHandler endPointHandler;
            if (PacketHandlers.TryGetValue(_id, out endPointHandler)) endPointHandler(_p);
            else NoHandlerMatchFor(_id);
        }

        public void NoHandlerMatchFor(int _packetId)
        {
            Debug.Log($"Client wasn't able to find PacketHandler with if - {_packetId}");
        }
    }

}