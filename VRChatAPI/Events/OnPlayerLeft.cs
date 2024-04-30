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

        public static OnPlayerLeft ProcessLog(dynamic eventHandler, string input)
        {
            var match = Regex.Match(input, @"OnPlayerLeft (.+)");
            if (match.Success)
            {
                var instance = new OnPlayerLeft
                {
                    Data = input,
                    DateTime = DateTime.Now,
                    DisplayName = match.Groups[1].Value
                };

                if (eventHandler != null)
                    eventHandler?.Invoke(null, instance);

                return instance;
            }
            return null;
        }
    }
}
