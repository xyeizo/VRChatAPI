using VRChatAPI.Modules;
using VRChatAPI.Objects;

namespace VRChatAPI.SDK
{
    public class VRChat
    {
        private VRChatConfig _vrChatConfig = new VRChatConfig();

        public LocalPlayer CurrentPlayer { get; set; }

        public LogFileMonitor LogFileMonitor { get; set; }

        public VRChat(VRChatConfig vRChatConfig = null)
        {
            if (vRChatConfig != null)
                _vrChatConfig = vRChatConfig;

            CurrentPlayer = new LocalPlayer();

            if (_vrChatConfig.MonitorLogFile)
                LogFileMonitor = new LogFileMonitor(this);
        }



    }
}
