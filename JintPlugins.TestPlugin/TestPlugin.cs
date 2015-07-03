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
        public string Test(string p)
        {
            Console.WriteLine("TestPlugin: {0}", p);

            return p + ", Added by TestPlugin";
        }
    }
}
