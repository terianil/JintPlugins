using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JintPlugins.Common;

namespace JintPlugins.TestPlugin
{
    public class TestPlugin : IPlugin
    {
        public void Test()
        {
            Console.WriteLine("TestPlugin3");
            //var trace = new StackTrace();

            //foreach (var frame in trace.GetFrames())
            //{
            //    Console.WriteLine("{0} as {1}", frame.GetMethod(), frame.GetFileName());
            //}

            throw new Exception("dupa");
        }
    }
}
