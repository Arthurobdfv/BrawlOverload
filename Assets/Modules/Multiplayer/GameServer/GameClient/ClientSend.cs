using GameServer;
using GameServer.Common;
using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{

    public class ClientSend : IClientSend
    {
        public void SendTCPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.tcp.SendData(_packet);
        }

        public void SendUDPData(Packet _packet)
        {
            _packet.WriteLength();
            Client.instance.udp.SendData(_packet);
        }

        #region Packets

        #endregion
    }

}