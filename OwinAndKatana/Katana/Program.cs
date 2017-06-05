using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Http;

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
                //app.UseHelloWorld();

                /* Middle ware examples */

                    // MiddleWare whichoutputs everything in the request context
                    //Lambda expression, takes the enviornment and the next component
                    //app.Use(async (Environment, next) =>
                    //{
                    //    foreach (var pair in Environment.Environment
                    //    ) // Enviornment.enviornment an IDictionary of string and object (Request context)
                    //    {
                    //        Console.WriteLine($"{pair.Key} {pair.Value}");
                    //    }
                    //    await next(); // chaining to the next component in the pipeline
                    //});

                    app.Use(async (Environment, next) =>
                    {
                        Console.WriteLine($"Requeesting: {Environment.Request.Path}"); // Path during the request

                        await next(); // If you dont use this then the next component on the pipleline will not be called.

                        Console.WriteLine($"Response: {Environment.Response.StatusCode}"); // Response returned following request
                    });


                /* WebAPI - Serailze an object and output result */
                // Install - Package - IncludePreRelease Microsoft.ASPNet.WebApi.OwinSelfHost

                /* Working with IIS server package */
                //Install-Package -IncludePreRelease Microsoft.ASPNet.Owin.Host.SystemWeb
                // The package above installs a paclage which nows how to plug into the aspNet pipeline.

                ConfigureWebApi(app);

            }

            private void ConfigureWebApi(IAppBuilder app)
            {
                var config = new HttpConfiguration(); // Controls the routing rules / serializers and formatters

                // We use the HttpConfig for specifying the Routing rules
                config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});

                // Plugging in the WebApi component
                app.UseWebApi(config);


            }
        }

        //public static class AppBuilderExtensions
        //{
        //    public static void UseHelloWorld(this IAppBuilder app)
        //    {
        //        app.Use<HelloWorldComponent>();
        //    }
        //}


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
