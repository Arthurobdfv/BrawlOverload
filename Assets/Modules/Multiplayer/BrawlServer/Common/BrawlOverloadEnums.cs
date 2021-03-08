using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrawlServer.Common
{
    enum BrawlServerPackets
    {
        Welcome = 1,
        Connect = 2,
        PlayerMovement = 3
    }

    enum BrawlClientPackets
    {
        WelcomeRecieved = 1,
        ConnectRecieved = 2,
        PlayerMovement = 3
    }
}
