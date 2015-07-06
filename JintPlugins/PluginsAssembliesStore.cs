using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JintPlugins {
    public class PluginsAssembliesStore {
        public readonly ConcurrentDictionary<string, Assembly> Assemblies = new ConcurrentDictionary<string, Assembly>();

        public void AddOrUpdate(string path)
        {
            var a = LoadPluginAssembly(path);
            this.Assemblies.AddOrUpdate(path, a, (s, assembly) => a);
        }

        public void Remove(string assemblyPath) {
            Assembly deleted;
            this.Assemblies.TryRemove(assemblyPath, out deleted);
        }

        private Assembly LoadPluginAssembly(string path) {
            var pdbPath = Path.ChangeExtension(path, ".pdb");

            if (!File.Exists(pdbPath)) {
                pdbPath = path;
            }

            return Assembly.Load(File.ReadAllBytes(path), File.ReadAllBytes(pdbPath));
        }
    }
}
