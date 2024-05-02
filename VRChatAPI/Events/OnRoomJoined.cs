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
        public string RoomInstance { get; set; } = string.Empty;

        public static OnRoomJoined ProcessLog(dynamic eventHandler, string input)
        {
            var match = Regex.Match(input, @"Joining wrld_(.+):(\d+)(.*)");
            if (match.Success)
            {
                var instance = new OnRoomJoined
                {
                    Data = input,
                    DateTime = DateTime.Now,
                    WorldId = "wrld_" + match.Groups[1].Value,
                    RoomInstance = match.Groups[2].Value + match.Groups[3].Value
                };

                if (eventHandler != null)
                    eventHandler?.Invoke(null, instance);

                return instance;
            }
            return null;
        }

        public bool IsValid()
        {
            if (!string.IsNullOrEmpty(WorldId) && !string.IsNullOrEmpty(RoomInstance))
                return true;

            return false;
        }
    }
}
