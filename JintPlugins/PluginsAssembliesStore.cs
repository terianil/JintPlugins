using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JintPlugins {
    internal static class PluginsAssembliesStore {
        internal static readonly ConcurrentDictionary<string, Assembly> Assemblies = new ConcurrentDictionary<string, Assembly>();

        public static void AddOrUpdate(string path)
        {
            var a = LoadPluginAssembly(path);
            Assemblies.AddOrUpdate(path, a, (s, assembly) => a);
        }

        public static void Remove(string assemblyPath) {
            Assembly deleted;
            Assemblies.TryRemove(assemblyPath, out deleted);
        }

        private static Assembly LoadPluginAssembly(string path) {
            var pdbPath = Path.ChangeExtension(path, ".pdb");

            if (!File.Exists(pdbPath)) {
                pdbPath = path;
            }

            return Assembly.Load(File.ReadAllBytes(path), File.ReadAllBytes(pdbPath));
        }
    }
}
