using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatAPI.Events;

namespace VRChatAPI.Objects
{
    public class LocalPlayer
    {
        public bool IsGameRunning { get; set; } = false;
        public 
        public OnRoomJoined? Location { get; set; } = null;
        public HashSet<string> CurrentPlayers { get; set; } = new HashSet<string>();
    }
}
