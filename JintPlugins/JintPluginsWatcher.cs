using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JintPlugins {
    internal class JintPluginsWatcher {
        internal JintPluginsWatcher(string pluginPath) {
            var watcher = new FileSystemWatcher(pluginPath, "*.dll") { EnableRaisingEvents = true };

            watcher.Created += PluginCreatedOrChanged;
            watcher.Changed += PluginCreatedOrChanged;
            watcher.Deleted += PluginDeleted;
        }

        private void PluginDeleted(object sender, FileSystemEventArgs e) {
            PluginsAssembliesStore.Remove(e.FullPath);
        }

        private void PluginCreatedOrChanged(object sender, FileSystemEventArgs e) {
            Thread.Sleep(1000);
            PluginsAssembliesStore.AddOrUpdate(e.FullPath);
        }
    }
}
