using System.Collections.Generic;
using System.IO;

namespace JintPlugins {
    public class JintPluginsStore
    {
        private IDictionary<string, JintPluginsWatcher> pluginsWatchers;

        public PluginsAssembliesStore PluginsAssembliesStore { get; private set; }

        public JintPluginsStore()
        {
            this.PluginsAssembliesStore = new PluginsAssembliesStore();
            this.pluginsWatchers = new Dictionary<string, JintPluginsWatcher>();
        }

        public void StartUpPlugins(string pluginPath) {            
            this.LoadExistingPlugins(pluginPath);
            this.pluginsWatchers[pluginPath] = new JintPluginsWatcher(pluginPath, this.PluginsAssembliesStore);
        }

        private void LoadExistingPlugins(string pluginPath) {
            var plugins = Directory.GetFiles(pluginPath, "*.dll");

            foreach (var plugin in plugins) {
                this.PluginsAssembliesStore.AddOrUpdate(plugin);
            }
        }
    }
}
