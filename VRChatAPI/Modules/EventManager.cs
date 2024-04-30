using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatAPI.Events;

namespace VRChatAPI.Modules
{
    public class EventManager
    {
        private static Dictionary<string, Action<string>> eventHandlers = new Dictionary<string, Action<string>>
        {
            { "Successfully left room", (input) => OnRoomLeft.ProcessLog(OnRoomLeftEvent, input) },
            { "Joining wrld_", (input) => OnRoomJoined.ProcessLog(OnRoomJoinedEvent, input) },
            { "OnPlayerLeft", (input) => OnPlayerLeft.ProcessLog(OnPlayerLeftEvent, input) },
            { "OnPlayerJoined", (input) => OnPlayerJoined.ProcessLog(OnPlayerJoinedEvent, input) }
        };

        public static event EventHandler<OnRoomLeft> OnRoomLeftEvent;
        public static event EventHandler<OnRoomJoined> OnRoomJoinedEvent;
        public static event EventHandler<OnPlayerLeft> OnPlayerLeftEvent;
        public static event EventHandler<OnPlayerJoined> OnPlayerJoinedEvent;

        public Task HandleEvent(string input)
        {
            foreach (var handler in eventHandlers)
            {
                if (input.Contains(handler.Key))
                {
                    handler.Value.Invoke(input);
                    break;
                }
            }
            return Task.CompletedTask;
        }
    }
}
