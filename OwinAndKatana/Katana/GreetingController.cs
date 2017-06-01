using System.Web.Http;

namespace Katana
{
    public class GreetingController : ApiController
    {
        public Greeting Get()
        {
            return new Greeting {Text = "Hello Paddy !!"};
        }
    }
}