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

        public event EventHandler<OnRoomLeft> Event;

        public void ProcessLog(string input)
        {
            if (input.Contains("Successfully left room"))
            {
                Data = input;

                Event?.Invoke(this, this);
            }
        }
    }
}
