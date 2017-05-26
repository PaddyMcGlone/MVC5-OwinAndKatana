using Microsoft.Owin.Hosting;
using Owin;
using System;

namespace Katana
{
    class Program
    {
        // Entry point
        static void Main(string[] args)
        {
            //Start the server
            string uri = "http://localhost:58146";

            using (WebApp.Start<Startup>(uri))
            {
                Console.WriteLine("Started!");
                Console.ReadKey();
                Console.WriteLine("Stopping");
            }
        }

        public class Startup
        {
            // IAppBuilder is important - defines how an application is going to behave and respond to http requests
            public void Configuration(IAppBuilder app)
            {
                // Run method - katana calls into to process all http requests
                app.Run(ctx => ctx.Response.WriteAsync("Hello World")); //WriteAsync, this is a method which returns a task.
            }
        }

    }
}
