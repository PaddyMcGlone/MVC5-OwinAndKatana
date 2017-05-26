using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
                //The two examples below are wrappers for components which can use AppFunc to do some work.

                // Add new page via the addition of diagnostics package
                //app.UseWelcomePage();

                // Run method - katana calls into to process all http requests
                //app.Run(ctx => ctx.Response.WriteAsync("Hello World")); //WriteAsync, this is a method which returns a task.

                //Register low level component
                AppBuilderUseExtensions.Use<HelloWorldComponent>();
            }
        }


        // Low level code you can write
        public class HelloWorldComponent
        {

            // In this code we have to define the next component in the pipeline.
            AppFunc _next;
            public HelloWorldComponent(AppFunc nextComponent)
            {
                _next = nextComponent; //Give me the next component in the pipeline
            }

            // Katana finds this via reflection which matches the AppFunc signature.
            public Task Invoke(IDictionary<string, object> enviornment)
            {
                // Invoke next item on the chain and pass on the dictionary
                //await _next(enviornment);

                // In the pipeline if we want to end everything and send something back to the 
                // client we can do this with the responsebody component.
                var response = enviornment["owin.ResponseBody"] as Stream;
                using (var writer = new StreamWriter(response))
                {
                    return writer.WriteAsync("Hello!!");
                }
            }
        }

    }
}
