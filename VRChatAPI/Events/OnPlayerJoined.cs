using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VRChatAPI.Events
{
    public class OnPlayerJoined
    {
        public string Data { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string DisplayName { get; set; } = string.Empty;

        public event EventHandler<OnPlayerJoined> Event;

        public void ProcessLog(string input)
        {
            var match = Regex.Match(input, @"OnPlayerJoined (.+)");
            if (match.Success)
            {
                DisplayName = match.Groups[1].Value;
                Event?.Invoke(this, this);
            }
        }
    }
}
