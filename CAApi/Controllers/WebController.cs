using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace CAApi.Controllers
{
    [Route("/Api/[controller]")]
    [ApiVersion("1.0")]
    public class WebController : Controller
    {
        /*
        [HttpGet("{page}", Name = nameof(GetPage))]
        public async Task<HttpResponseMessage> GetPage(String page, CancellationToken ct)
        {
            string fileContents = System.IO.File.ReadAllText(@"Web\Main.html");
            var response = new HttpResponseMessage();
            response.Content = new StringContent(fileContents);
            //response.Content = new Html
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
        */

    }
}