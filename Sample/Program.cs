using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jint;
using JintPlugins;

namespace Sample
{
    class Program
    {
        private static readonly string PluginPath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "..", "..", "..", "JintPlugins.TestPlugin", "bin", "Debug");

        static void Main(string[] args)
        {
            //toAppStart
            var plugins = new JintPluginsStore();
            plugins.StartUpPlugins(PluginPath);

            while (true)
            {
                Transform(plugins);
                Thread.Sleep(1000);
            }
        }

        private static void Transform(JintPluginsStore plugins)
        {
            var engine = new Engine();
            engine.UsePlugins("ais", plugins);

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
    }
}
