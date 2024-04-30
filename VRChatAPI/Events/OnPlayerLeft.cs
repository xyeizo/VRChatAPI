using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VRChatAPI.Events
{
    public class OnPlayerLeft
    {
        public string Data { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string DisplayName { get; set; } = string.Empty;

        public event EventHandler<OnPlayerLeft> Event;

        public void ProcessLog(string input)
        {
            var match = Regex.Match(input, @"OnPlayerLeft (.+)");
            if (match.Success)
            {
                DisplayName = match.Groups[1].Value;
                Event?.Invoke(this, this);
            }
        }
    }
}
