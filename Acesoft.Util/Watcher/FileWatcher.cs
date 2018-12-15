using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Acesoft.Util
{
    public class FileWatcher
    {
        readonly ConcurrentDictionary<string, bool> _events = new ConcurrentDictionary<string, bool>();
        readonly IList<FileSystemWatcher> _watchers;

        public FileWatcher()
        {
            _watchers = new List<FileSystemWatcher>();
        }

        public void Watch(string file, Action<FileInfo> action)
        {
            var fileInfo = new FileInfo(file);
            var watcher = new FileSystemWatcher(fileInfo.DirectoryName)
            {
                Filter = fileInfo.Name,
                NotifyFilter = NotifyFilters.LastWrite
            };

            watcher.Changed += (sender, e) =>
            {
                Task.Run(() =>
                {
                    _events.GetOrAdd(file, (_) =>
                    {
                        action(fileInfo);
                        return true;
                    });

                    if (_events.ContainsKey(file) && _events[file])
                    {
                        _events[file] = false;

                        Thread.Sleep(500);
                        _events.TryRemove(file, out bool b);
                    }
                });
            };

            watcher.EnableRaisingEvents = true;
            _watchers.Add(watcher);
        }

        public void Clear()
        {
            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            _watchers.Clear();
        }
    }
}
