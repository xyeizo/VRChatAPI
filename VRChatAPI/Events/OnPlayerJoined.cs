using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VRChatAPI.SDK;

namespace VRChatAPI.Events
{
    public class OnPlayerJoined
    {
        public string Data { get; set; } = string.Empty;
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string DisplayName { get; set; } = string.Empty;

        public static OnPlayerJoined ProcessLog(dynamic eventHandler, string input)
        {
            var match = Regex.Match(input, @"OnPlayerJoined (.+)");
            if (match.Success)
            {
                var instance = new OnPlayerJoined() 
                { 
                    Data = input, 
                    DisplayName = match.Groups[1].Value, 
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
