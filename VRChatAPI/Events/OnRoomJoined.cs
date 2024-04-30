using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VRChatAPI.Events
{
    public class OnRoomJoined
    {
        public string Data { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string WorldId { get; set; } = string.Empty;
        public string RoomInstance { get; set; }

        public event EventHandler<OnRoomJoined> Event;

        public OnRoomJoined ProcessLog(string input)
        {
            var match = Regex.Match(input, @"Joining wrld_(.+):(\d+)");
            if (match.Success)
            {
                Data = input;
                WorldId = match.Groups[1].Value;
                RoomInstance = match.Groups[2].Value;

                Event?.Invoke(this, this);
                return this;
            }
            return null;
        }
    }
}
