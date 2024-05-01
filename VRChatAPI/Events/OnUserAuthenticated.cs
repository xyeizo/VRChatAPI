using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VRChatAPI.Events
{
    public class OnUserAuthenticated
    {
        public string Username { get; set; }
        public string UserID { get; set; }
        public string AvatarID { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public static OnUserAuthenticated ProcessLog(dynamic eventHandler, string logEntry)
        {
            var userMatch = Regex.Match(logEntry, @"User Authenticated: (.+) \(usr_");
            var userIdMatch = Regex.Match(logEntry, @"(usr_[\w\d-]+)");
            var avatarMatch = Regex.Match(logEntry, @"avatar: (avtr_[\w\d-]+)");

            if (userMatch.Success && userIdMatch.Success && avatarMatch.Success)
            {
                var instance = new OnUserAuthenticated
                {
                    Username = userMatch.Groups[1].Value.Trim(),
                    AvatarID = avatarMatch.Groups[1].Value,
                    UserID = userIdMatch.Groups[1].Value,
                    DateTime = DateTime.Now,
                };

                if (eventHandler != null)
                    eventHandler?.Invoke(null, instance);

                return instance;
            }
            return null;
        }
    }
}
