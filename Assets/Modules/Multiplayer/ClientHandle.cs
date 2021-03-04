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

    public static void SpawnPlayer(Packet _packet)
    {
        int id = _packet.ReadInt();
        string userName = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(id, userName, _position, _rotation);
    }
}
