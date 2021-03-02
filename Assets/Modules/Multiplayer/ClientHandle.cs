using GameServer;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet packet)
    {
        var msg = packet.ReadString();
        var value = packet.ReadInt();

        Debug.Log($"MessageFromServer: {msg}, id {value}");
        Client.instance.myId = value;
        ClientSend.WelcomeRecieved();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    internal static void UdpTest(Packet packet)
    {
        var msg = packet.ReadString();
        Debug.Log($"Recieved packet via UDP. Contains Message: {msg}");
        ClientSend.UDPTestRecieve();
    }
}
