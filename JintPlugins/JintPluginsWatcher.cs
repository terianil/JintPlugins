using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JintPlugins {
    internal class JintPluginsWatcher {
        private readonly PluginsAssembliesStore store;

        internal JintPluginsWatcher(string pluginPath, PluginsAssembliesStore store) {
            this.store = store;
            var watcher = new FileSystemWatcher(pluginPath, "*.dll") { EnableRaisingEvents = true };

            watcher.Created += PluginCreatedOrChanged;
            watcher.Changed += PluginCreatedOrChanged;
            watcher.Deleted += PluginDeleted;
        }

        private void PluginDeleted(object sender, FileSystemEventArgs e) {
            this.store.Remove(e.FullPath);
        }

        private void PluginCreatedOrChanged(object sender, FileSystemEventArgs e) {
            Thread.Sleep(1000);
            this.store.AddOrUpdate(e.FullPath);
        }
    }
}
