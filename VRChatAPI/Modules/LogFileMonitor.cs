﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VRChatAPI.Events;
using VRChatAPI.SDK;

namespace VRChatAPI.Modules
{
    public class LogFileMonitor : IDisposable
    {
        private readonly string _logFileDirectory;
        public FileInfo? LogFile;
        private long _lastPosition = 0;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

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
            Thread logThread = new Thread(async () => await MonitorLogFile(_cancellationTokenSource.Token));
            logThread.IsBackground = true;
            logThread.Start();
        }

        private async Task MonitorLogFile(CancellationToken cancellationToken)
        {
            try
            {
                using (FileStream fileStream = new FileStream(LogFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.Default))
                {
                    InitializeCurrentRoom(fileStream, streamReader);
                    InitializeInstancePlayers(fileStream, streamReader);

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (fileStream.Length > _lastPosition)
                        {
                            fileStream.Seek(_lastPosition, SeekOrigin.Begin);
                            string content = await streamReader.ReadToEndAsync();
                            Interlocked.Exchange(ref _lastPosition, fileStream.Position);

                            await vrChatInstance.EventManager.HandleEvent(content);
                        }
                        await Task.Delay(500, cancellationToken);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                
            }
            catch (Exception ex)
            {
                
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

            bool hasLeftAfterJoin = false;

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                if (lines[i].Contains("Successfully left room") && !hasLeftAfterJoin)
                {
                    hasLeftAfterJoin = true;
                }
                else if (lines[i].Contains("Joining wrld_"))
                {
                    if (!hasLeftAfterJoin)
                    {
                        var eventArgs = OnRoomJoined.ProcessLog(null, lines[i]);
                        vrChatInstance.CurrentPlayer.Location = eventArgs;
                        break;
                    }
                    hasLeftAfterJoin = false;
                }
            }

            Interlocked.Exchange(ref _lastPosition, fileStream.Length);
        }

        public void InitializeInstancePlayers(FileStream fileStream, StreamReader streamReader)
        {
            if (vrChatInstance.CurrentPlayer.Location == null ||
                string.IsNullOrEmpty(vrChatInstance.CurrentPlayer.Location.WorldId) ||
                string.IsNullOrEmpty(vrChatInstance.CurrentPlayer.Location.RoomInstance))
            {
                return;
            }

            fileStream.Seek(0, SeekOrigin.Begin);
            string content = streamReader.ReadToEnd();
            string[] lines = content.Split('\n');

            var currentPlayers = new HashSet<string>();
            Regex joinRegex = new Regex(@"OnPlayerJoined (.+)");
            Regex leaveRegex = new Regex(@"OnPlayerLeft (.+)");

            foreach (string line in lines)
            {
                var joinMatch = joinRegex.Match(line);
                if (joinMatch.Success)
                {
                    string playerName = joinMatch.Groups[1].Value.Trim();
                    currentPlayers.Add(playerName);
                }

                var leaveMatch = leaveRegex.Match(line);
                if (leaveMatch.Success)
                {
                    string playerName = leaveMatch.Groups[1].Value.Trim();
                    currentPlayers.Remove(playerName);
                }
            }

            vrChatInstance.CurrentPlayer.CurrentPlayers = currentPlayers;
        }

        private void InitializeCurrentUser(FileStream fileStream, StreamReader streamReader)
        {
            fileStream.Seek(0, SeekOrigin.Begin); // Start reading from the beginning of the file
            string content = streamReader.ReadToEnd();
            string[] lines = content.Split('\n');

            foreach (string line in lines)
            {
                var userAuthenticated = OnUserAuthenticated.ProcessLog(null, line);
                if (!string.IsNullOrEmpty(userAuthenticated.Username))
                {
                    CurrentUser = userAuthenticated;
                    break;
                }
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
    }
}
