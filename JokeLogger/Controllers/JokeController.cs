using JokeLogger.Models;
using JokeLogger.Repository;
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
    [Route("api/joke")]
    [ApiController]
    public class JokeController : ControllerBase
    {
        private readonly ILogJokeRepository _logJokeRepository;
        //Hosted web API REST Service base url
        string Baseurl = "https://icanhazdadjoke.com/";

        public JokeController(ILogJokeRepository logJokeRepository)
        {
            _logJokeRepository = logJokeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Joke>> Get()
        {
            Joke newJoke = new();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "My Library (https://github.com/bjarmstr/JokeLoggerApp)");

                //Sending request to find web api REST service resource GetAllJokes using HttpClient
                //in the "" can go additional url folders
                HttpResponseMessage Res = await client.GetAsync("");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var jokeResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Joke list
                    newJoke = JsonConvert.DeserializeObject<Joke>(jokeResponse);
                }
                //put joke in correct format for logger
                LogJoke logJoke = new();
                logJoke.Joke = newJoke.joke;
                logJoke.DateRequested = DateTime.UtcNow;

                //put joke into database
               await _logJokeRepository.Create(logJoke);


                return newJoke;
            }
        }
        [HttpGet("count")]
        public async Task<ActionResult<List<Joke>>> Get(int count)
        {
            List<Joke> Jokes = new();
            Joke newJoke = new();
            for (int i = 0; i < count; i++)
            {

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
                    //in the "" can go additional url folders
                    HttpResponseMessage Res = await client.GetAsync("");
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var jokeResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Joke list
                        newJoke = JsonConvert.DeserializeObject<Joke>(jokeResponse);
                    }

                }
                Jokes.Add(newJoke);
                //put joke in correct format for logger
                LogJoke logJoke = new(); //I am creating a new logJoke everytime with same name
                logJoke.Joke = newJoke.joke;
                logJoke.DateRequested = DateTime.UtcNow;

                //put joke into database
                await _logJokeRepository.Create(logJoke);
            }
            return Jokes;
        }
            [HttpGet("log/{id}")]
            public async Task<ActionResult<LogJoke>> GetLoggedJoke(int id){
                return await _logJokeRepository.Get(id);
            }

        [HttpGet("log/count")]
        public async Task<IEnumerable<LogJoke>> GetLoggedJokes()
        {
            return await _logJokeRepository.Get();
          
        }

        [HttpPost]
            public async Task<ActionResult<LogJoke>>PostLogJokes([FromBody]LogJoke logJoke)
            {
                var newLogJoke = await _logJokeRepository.Create(logJoke);
            return CreatedAtAction(nameof(GetLoggedJoke), new { id = newLogJoke.Id }, newLogJoke);
            }
        

    }
}
