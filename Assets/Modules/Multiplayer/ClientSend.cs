﻿using GameServer;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeRecieved()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.userNameField.text);

            SendTCPData(_packet);
        }
    }

    internal static void UDPTestRecieve()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestRecieved))
        {
            _packet.Write("Recieved a UDP Packet.");
            SendUDPData(_packet);
        }
    }
    #endregion
}
