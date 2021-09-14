using JokeLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JokeLogger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        //Hosted web API REST Service base url
        string Baseurl = "https://icanhazdadjoke.com/";

        [HttpGet]
        public async Task<ActionResult<Joke>> Get()
        {
            Joke EmpInfo = new();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "My Library (https://github.com/bjarmstr/JokeLoggerApp)");

                //Sending request to find web api REST service resource GetAllJokes using HttpClient

                HttpResponseMessage Res = await client.GetAsync("");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Joke list
                    EmpInfo = JsonConvert.DeserializeObject<Joke>(EmpResponse);
                }
                //returning the employee to view
                return EmpInfo;
            }
        }
    }
}
