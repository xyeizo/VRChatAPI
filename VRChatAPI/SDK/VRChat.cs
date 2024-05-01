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

            if (_vrChatConfig.MonitorLogFile)
                LogFileMonitor = new LogFileMonitor(this);

            CurrentPlayer = new LocalPlayer();
            EventManager = new EventManager();
        }

        public Task InitializeAsync()
        {
             if (_vrChatConfig.MonitorLogFile)
                LogFileMonitor.StartMonitoring();

             return Task.CompletedTask;
        }

        public bool IsRunning() => Process.GetProcessesByName("VRChat").Any();
    }
}
