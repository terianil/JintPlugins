using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jint;
using JintPlugins.Common;

namespace JintPlugins {
    public static class JintExtensions {
        public static void UsePlugins(this Engine engine, string pluginsNamespace)
        {
            var plugins = LoadPluginsInstances().Cast<IPlugin>();
            var plugObject = plugins.ToDictionary(x => x.GetType().Name, x => x);

            engine.SetValue(pluginsNamespace, plugObject);
        }

        private static IEnumerable<object> LoadPluginsInstances()
        {
            return PluginsAssembliesStore.Assemblies.Select(assembly => assembly.Value
                .GetTypes()
                .Where(p => typeof(IPlugin).IsAssignableFrom(p))).SelectMany(plugins => plugins.Select(Activator.CreateInstance));
        }
    }
}
