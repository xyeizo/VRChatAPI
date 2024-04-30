using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRChatAPI.Events
{
    public class OnRoomLeft
    {
        public string Data { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;

        public static OnRoomLeft ProcessLog(dynamic eventHandler, string input)
        {
            if (input.Contains("Successfully left room"))
            {
                var instance = new OnRoomLeft 
                { 
                    Data = input, 
                    DateTime = DateTime.Now 
                };

                if (eventHandler != null)
                    eventHandler?.Invoke(null, instance);

                return instance;
            }
            return null;
        }
    }
}
