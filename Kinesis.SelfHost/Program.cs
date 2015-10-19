using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;

namespace Kinesis.SelfHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string uri = "http://localhost:8080/";

            using (WebApp.Start<Startup>(uri))
            {
                System.Console.WriteLine("Running Web Api Host. Press any key to exit");
                System.Console.ReadKey();
                System.Console.WriteLine("Exiting...");
            }
        }
    }
}
