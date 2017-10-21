using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRSServerHttp.Server.Websocket
{
    public class ReadState
    {
        public ReadState()
        {
            Data = new List<byte>();
        }
        public List<byte> Data { get; private set; }
        public FrameType? FrameType { get; set; }
        public void Clear()
        {
            Data.Clear();
            FrameType = null;
        }
    }
}
