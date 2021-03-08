

using BrawlServer.Common;
using GameClient;
using GameServer.Common;
using System;

namespace BrawlServer
{
    class BrawlClientSender : ClientSend
    {
        public void WelcomeRecieved()
        {
            using (Packet _packet = new Packet((int)BrawlClientPackets.WelcomeRecieved))
            {
                _packet.Write(Client.instance.myId);
                _packet.Write(UIManager.instance.userNameField.text);

                SendTCPData(_packet);
            }
        }

        public void PlayerMovement(bool[] inputs)
        {
            using (Packet _p = new Packet((int)BrawlClientPackets.PlayerMovement))
            {
                _p.Write(inputs.Length);
                foreach(var input in inputs)
                {
                    _p.Write(input);
                }
                _p.Write(GameManager.players[Client.instance.myId].transform.rotation);
                SendUDPData(_p);
            }
        }
    }
}
