using GameServer;
using GameServer.Common;

public interface IClientSend
{
    void SendTCPData(Packet _packet);
    void SendUDPData(Packet _packet);
}