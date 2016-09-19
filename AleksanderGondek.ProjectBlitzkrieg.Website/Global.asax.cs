using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using GlobalConfiguration = System.Web.Http.GlobalConfiguration;

namespace AleksanderGondek.ProjectBlitzkrieg.Website
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            InitializeOrleans();

            // Code that runs on application startup
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        private void InitializeOrleans()
        {
            var errorCounter = 0;
            while (errorCounter <= 10)
            {
                try
                {
                    GrainClient.Initialize(ClientConfiguration.LocalhostSilo());
                    return;
                }
                catch (SiloUnavailableException)
                {
                    errorCounter++;
                }
                catch (HttpException)
                {
                    errorCounter++;
                }

                Thread.Sleep(15000); // Sleep for 15 secs
            }

            throw new Exception("Too many unsuccessfull attempts to setup Orleans client!");
        }
    }
}