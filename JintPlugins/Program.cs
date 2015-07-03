using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jint;
using JintPlugins.Common;

namespace JintPlugins
{
    class Program
    {
        private static readonly string PluginPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "..", "..", "..", "JintPlugins.TestPlugin", "bin", "Debug");// @"C:\Users\kmaciejczek\documents\visual studio 2013\Projects\JintPlugins\JintPlugins.TestPlugin\bin\Debug\";
        private static ConcurrentDictionary<string, Assembly> assemblies = new ConcurrentDictionary<string, Assembly>();

        static void Main(string[] args)
        {

            var trace = new StackTrace();


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

            var plugObject = plugins.ToDictionary(x => x.GetType().Name, x => x);

            engine.SetValue("ais", plugObject);

            //foreach (var plugin in plugins)
            //{                
            //    engine.SetValue("ais." + plugin.GetType().Name, plugin);
            //}

            engine.Execute(@"
                function hello() {

                return ais.TestPlugin.Test('dsadsa');
                };
            ");

            try
            {
                var hello = engine.Invoke("hello");
                Console.WriteLine(hello);
            }
            catch (TargetInvocationException e)
            {
                ExceptionDispatchInfo.Capture(e.InnerException).Throw();
            }
        }

        private static void StartUpPlugins()
        {
            var plugins = Directory.GetFiles(PluginPath, "*.dll");

            foreach (var plugin in plugins)
            {
                var a = LoadPluginAssembly(plugin);
                assemblies.AddOrUpdate(plugin, a, (s, assembly) => a);
            }
        }

        private static Assembly LoadPluginAssembly(string path)
        {
            var pdbPath = Path.ChangeExtension(path, ".pdb");

            if (!File.Exists(pdbPath))
            {
                pdbPath = path;
            }

            return Assembly.Load(File.ReadAllBytes(path), File.ReadAllBytes(pdbPath));
        }

        private static FileSystemWatcher RegisterWatcher()
        {
            var watcher = new FileSystemWatcher(PluginPath, "*.dll") { EnableRaisingEvents = true };

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
            Thread.Sleep(1000);
            var a = LoadPluginAssembly(e.FullPath);
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
