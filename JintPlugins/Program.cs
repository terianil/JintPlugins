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
    class Program {
        private const string PluginPath = @"C:\Users\kmaciejczek\documents\visual studio 2013\Projects\JintPlugins\JintPlugins.TestPlugin\bin\Debug\";
        private static ConcurrentDictionary<string, Assembly> assemblies = new ConcurrentDictionary<string, Assembly>();

        static void Main(string[] args) {
            

            //toAppStart
            StartUpPlugins();
            RegisterWatcher();

            while (true)
            {
                Transform();
                Thread.Sleep(1000);
            }
        }

        private static void Transform()
        {
            var plugins = LoadPlugins().Cast<IPlugin>();

            var engine = new Engine();
            
            engine.SetValue("log", new Action<object>(Console.WriteLine));

            foreach (var plugin in plugins)
            {
                engine.SetValue(plugin.GetType().Name, plugin);
            }

            engine.Execute(@"
                function hello() {

                TestPlugin.Test();

                return 'dsadsa';
                };
            ");

            var hello = engine.Invoke("hello");
            Console.WriteLine(hello);
        }

        private static void StartUpPlugins()
        {
            var plugins = Directory.GetFiles(PluginPath, "*.dll");

            foreach (var plugin in plugins)
            {
                var a = Assembly.Load(File.ReadAllBytes(plugin));
                assemblies.AddOrUpdate(plugin, a, (s, assembly) => a);
            }
        }

        private static FileSystemWatcher RegisterWatcher()
        {
            var watcher = new FileSystemWatcher(PluginPath, "*.dll"){ EnableRaisingEvents = true };

            watcher.Created += PluginCreatedOrChanged;
            watcher.Changed += PluginCreatedOrChanged;  
            watcher.Deleted += PluginDeleted;

            return watcher;
        }

        private static void PluginDeleted(object sender, FileSystemEventArgs e)
        {
            Assembly deleted;
            assemblies.TryRemove(e.FullPath, out deleted);
        }

        private static void PluginCreatedOrChanged(object sender, FileSystemEventArgs e)
        {
            var a = Assembly.Load(File.ReadAllBytes(e.FullPath));
            assemblies.AddOrUpdate(e.FullPath, a, (s, assembly) => a);
        }

        public static IEnumerable<object> LoadPlugins()
        {
            foreach (var assembly in assemblies)
            {
                var plugins = assembly.Value
                .GetTypes()
                .Where(p => typeof(IPlugin).IsAssignableFrom(p));

                var materializedPlugins = plugins.Select(Activator.CreateInstance);


                foreach (var materializedPlugin in materializedPlugins)
                {
                    yield return materializedPlugin;
                }
            }
        }
    }
}
