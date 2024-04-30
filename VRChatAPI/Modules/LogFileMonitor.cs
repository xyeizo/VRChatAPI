using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChatAPI.Events;
using VRChatAPI.SDK;

namespace VRChatAPI.Modules
{
    public class LogFileMonitor
    {
        private readonly string _logFileDirectory;
        public FileInfo? LogFile;
        private long _lastPosition = 0;

        private VRChat vrChatInstance { get; set; }

        public LogFileMonitor(VRChat instance)
        {
            vrChatInstance = instance;

            string localLowPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "..", "LocalLow");
            _logFileDirectory = Path.Combine(localLowPath, "VRChat", "VRChat");

            LogFile = GetLogFile();
        }

        public void StartMonitoring()
        {
            Thread logThread = new Thread(async () => await MonitorLogFile());
            logThread.IsBackground = true;
            logThread.Start();
        }

        private async Task MonitorLogFile()
        {
            using (FileStream fileStream = new FileStream(LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
            {
                InitializeCurrentRoom(fileStream, streamReader);

                while (true)
                {
                    if (fileStream.Length > _lastPosition)
                    {
                        fileStream.Seek(_lastPosition, SeekOrigin.Begin);
                        string content = await streamReader.ReadToEndAsync();
                        _lastPosition = fileStream.Position;

                        await EventManager.HandleEvemt(content);
                    }
                    await Task.Delay(500);
                }
            }
        }

        public FileInfo? GetLogFile()
        {
            return new DirectoryInfo(_logFileDirectory)
                .GetFiles("*.txt")
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();
        }

        private void InitializeCurrentRoom(FileStream fileStream, StreamReader streamReader)
        {
            fileStream.Seek(0, SeekOrigin.Begin);
            string content = streamReader.ReadToEnd();
            string[] lines = content.Split('\n');

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].Contains("Joining wrld_"))
                {
                    var eventArgs = new OnRoomJoined().ProcessLog(lines[i]);
                    vrChatInstance.CurrentPlayer.Location = eventArgs;
                    break;
                }
            }

            _lastPosition = fileStream.Length;
        }
    }
}
