using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Host.HttpListener;
using Newtonsoft.Json;
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

        private static void AwsConfig(IAppBuilder app) 
        {
            var credentials = "EAAAAL0e89XAViOo+JWOgBvjWdhrs4DlgRucUgsUtmR3RlX2JcfGbRPN+8P7s5I2R3kKgHUmv85cTCY5wG1d8pSK2q4z+PBhcJ3HfdH2hgr1BBEZm1gaFvFZQa1KDvP+hGnOg06ULzL7LV/8bY5N1XuJhFI=";
            var json = Crypto.DecryptStringAES(credentials, "objectsharp");

            dynamic aws = JsonConvert.DeserializeObject(json);
            Amazon.Util.ProfileManager.RegisterProfile(ConfigurationManager.AppSettings["AWSProfileName"], aws.key, aws.secret);
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
