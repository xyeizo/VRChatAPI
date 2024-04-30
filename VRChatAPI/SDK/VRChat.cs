using System.Diagnostics;
using VRChatAPI.Events;
using VRChatAPI.Modules;
using VRChatAPI.Objects;

namespace VRChatAPI.SDK
{
    public class VRChat
    {
        private VRChatConfig _vrChatConfig = new VRChatConfig();

        public LocalPlayer CurrentPlayer { get; set; }

        public LogFileMonitor LogFileMonitor { get; set; }
        public EventManager EventManager { get; set; }

        public VRChat(VRChatConfig vRChatConfig = null)
        {
            if (vRChatConfig != null)
                _vrChatConfig = vRChatConfig;

            CurrentPlayer = new LocalPlayer();

            if (_vrChatConfig.MonitorLogFile)
                LogFileMonitor = new LogFileMonitor(this);
        }


        public bool IsRunning() => Process.GetProcessesByName("VRChat").Any();
    }
}
