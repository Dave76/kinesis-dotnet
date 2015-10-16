using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Host.HttpListener;
using Owin;

namespace Kinesis.SelfHost
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            // Tips from @daniel
            FlipBat(appBuilder);

            appBuilder.UseWebApi(configuration);
        }

        private static void FlipBat(IAppBuilder app)
        {
            // Salt to taste:
            int requestQueueLimit = 5000;
            int maxAccepts = 5000;
            int maxRequests = 5000;

            // https://katanaproject.codeplex.com/workitem/161
            // http://stackoverflow.com/questions/15417062/changing-http-sys-kernel-queue-limit-when-using-net-httplistener
            var owinListener = (OwinHttpListener)app.Properties["Microsoft.Owin.Host.HttpListener.OwinHttpListener"];
            owinListener.SetRequestQueueLimit(requestQueueLimit);

            // Salt to taste
            owinListener.SetRequestProcessingLimits(maxAccepts, maxRequests);

            // Double check it
            owinListener.GetRequestProcessingLimits(out maxAccepts, out maxRequests);
            Console.WriteLine("Max Accepts: " + maxAccepts);
            Console.WriteLine("Max Requests: " + maxRequests);
        }
    }
}
