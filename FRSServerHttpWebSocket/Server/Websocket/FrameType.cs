using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRSServerHttp.Server.Websocket
{
    public enum FrameType : byte
    {
        Continuation,
        Text,
        Binary,
        Close = 8,
        Ping = 9,
        Pong = 10,
    }
}
