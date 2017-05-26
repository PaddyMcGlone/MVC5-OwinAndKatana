using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Katana
{
    // Using the AppFunc throughout instead of along name throughout
    using AppFunc = Func<IDictionary<string, object>, Task>;

    class Program
    {
        // Entry point
        static void Main(string[] args)
        {
            //Start the server
            string uri = "http://localhost:8080";

            using (WebApp.Start<Startup>(uri)) // Explicitly starting a web server (Uri listen at given location)
            {
                Console.WriteLine("Started!");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }

        // This method tells the server how to configure itself.
        public class Startup
        {
            // IAppBuilder is important - defines how an application is going to behave and respond to http requests
            public void Configuration(IAppBuilder app)
            {
                // Add new page via the addition of diagnostics package
                app.UseWelcomePage();

                // Run method - katana calls into to process all http requests
                //app.Run(ctx => ctx.Response.WriteAsync("Hello World")); //WriteAsync, this is a method which returns a task.
            }
        }

        public class HelloWorldComponent
        {

            // In this code we have to define the next component in the pipeline.
            AppFunc _next;
            public HelloWorldComponent(AppFunc nextComponent)
            {
                _next = nextComponent;
            }

            public async Task Invoke(IDictionary<string, object> enviornment)
            {
                await _next();
            }
        }

    }
}
