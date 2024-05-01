using System.Diagnostics;
using VRChatAPI.Events;
using VRChatAPI.Modules;
using VRChatAPI.Objects;

namespace VRChatAPI.SDK
{
    public class VRChat
    {
        private VRChatConfig _vrChatConfig = new VRChatConfig();

        public LocalPlayer CurrentPlayer { get; } = new LocalPlayer();
        public LogFileMonitor LogFileMonitor { get; private set; }
        public EventManager EventManager { get; } = new EventManager();

        public VRChat(VRChatConfig vrChatConfig = null)
        {
            _vrChatConfig = vrChatConfig ?? new VRChatConfig();
            LogFileMonitor = new LogFileMonitor(this);
        }

        public Task InitializeAsync()
        {
            if (_vrChatConfig.MonitorLogFile && LogFileMonitor != null)
                LogFileMonitor.StartMonitoring();

            return Task.CompletedTask;
        }

        public bool IsRunning() => Process.GetProcessesByName("VRChat").Any();
    }
}
