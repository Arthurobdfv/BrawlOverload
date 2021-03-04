using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using GameServer;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 3333;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private delegate void PacketHandler(Packet packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this);
    }

    private void Start()
    {
        tcp = new TCP();
        udp = new UDP();
    }

    public void ConnectedToServer()
    {
        InitializeClientData();
        tcp.Connect();
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] recieveBuffer;

        private Packet recievedData;

        public void Connect()
        {
            socket = new TcpClient()
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            recieveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);

        }

        private void ConnectCallback(IAsyncResult ar)
        {
            socket.EndConnect(ar);

            if (!socket.Connected) return;

            recievedData = new Packet();

            stream = socket.GetStream();
            stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
        }

        private void RecieveCallback(IAsyncResult ar)
        {
            try
            {
                int _byteLength = stream.EndRead(ar);
                if (_byteLength <= 0)
                {
                    //todo disconnect
                }
                byte[] data = new byte[_byteLength];
                Array.Copy(recieveBuffer, data, _byteLength);

                recievedData.Reset(HandleData(data));
                stream.BeginRead(recieveBuffer, 0, dataBufferSize, RecieveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error recieving TCP data: {e.Message}");
                //todo disconnect

            }
        }

        private bool HandleData(byte[] data)
        {
            var packetLength = 0;
            recievedData.SetBytes(data);
            if (recievedData.UnreadLength() >= 4)
            {
                packetLength = recievedData.ReadInt();
                if (packetLength <= 0) return true;
            }

            while (packetLength > 0 && packetLength <= recievedData.UnreadLength())
            {
                byte[] _packetBytes = recievedData.ReadBytes(packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                packetLength = 0;
                if (recievedData.UnreadLength() >= 4)
                {
                    packetLength = recievedData.ReadInt();
                    if (packetLength <= 0) return true;
                }
            }
            if (packetLength <= 1) return true;
            return false;
        }

        internal void SendData(Packet packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error sending data to server via TCP: {e.Message}");
            }
        }
    }

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);

            socket.BeginReceive(RecieveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Error sending data to server via UDP: { e.Message }");
            }
        }



        private void RecieveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(RecieveCallback, null);
                if (_data.Length < 4)
                {
                    //TODO Disconnect
                    return;
                }

                HandleData(_data);
            }
            catch (Exception e)
            {
                //todo disconnect
            }
        }

        private void HandleData(byte[] data)
        {
            using (Packet _packet = new Packet(data))
            {
                int _packetLength = _packet.ReadInt();
                data = _packet.ReadBytes(_packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }
    }
    private void InitializeClientData()
    {

        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer }
        };
        Debug.Log($"Initialized Packets.");
    }
}

