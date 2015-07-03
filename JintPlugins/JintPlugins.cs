using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JintPlugins.Common;

namespace JintPlugins {
    public class JintPlugins {
        private JintPluginsWatcher PluginsWatcher { get; set; }

        public void StartUpPlugins(string pluginPath) {
            this.LoadExistingPlugins(pluginPath);
            this.PluginsWatcher = new JintPluginsWatcher(pluginPath);
        }

        private void LoadExistingPlugins(string pluginPath) {
            var plugins = Directory.GetFiles(pluginPath, "*.dll");

            foreach (var plugin in plugins) {
                PluginsAssembliesStore.AddOrUpdate(plugin);
            }
        }
    }
}
