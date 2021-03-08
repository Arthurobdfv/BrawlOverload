using GameServer;
using GameServer.Common;
using System.Collections.Generic;

public interface IClientHandle
{
    Dictionary<int, GameClient.Client.PacketHandler> PacketHandlers { get; }

    void Handle(int _id, Packet _p);
}