using System;
using System.Collections.Generic;
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
            Console.WriteLine("TestPlugin1");
        }
    }
}
