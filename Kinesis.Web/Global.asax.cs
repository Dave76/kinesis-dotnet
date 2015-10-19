using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Newtonsoft.Json;

namespace Kinesis.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        private static void AwsConfig()
        {
            var credentials = "EAAAAL0e89XAViOo+JWOgBvjWdhrs4DlgRucUgsUtmR3RlX2JcfGbRPN+8P7s5I2R3kKgHUmv85cTCY5wG1d8pSK2q4z+PBhcJ3HfdH2hgr1BBEZm1gaFvFZQa1KDvP+hGnOg06ULzL7LV/8bY5N1XuJhFI=";
            var json = Crypto.DecryptStringAES(credentials, "objectsharp");

            dynamic aws = JsonConvert.DeserializeObject(json);
            Amazon.Util.ProfileManager.RegisterProfile(ConfigurationManager.AppSettings["AWSProfileName"], aws.key, aws.secret);
        }
    }
}
