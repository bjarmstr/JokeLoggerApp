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
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<Joke> MakeRequest()
        {
            Joke joke = new();

            using (var client = new HttpClient())
            {                //Passing service base url
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
                    joke = JsonConvert.DeserializeObject<Joke>(jokeResponse);
                }

                return joke;
            }
        }

        [HttpGet]

        public async Task<IActionResult> Get(int count = 1)
        {
            Joke newJoke = new();
           if (count == 1)
          {
                newJoke = await MakeRequest();
                return Ok(newJoke);
            }
            else
            {
                List<Joke> Jokes = new();
                for (int i = 0; i < count; i++)
                {
                    //I would like to have a maximum count that will be retrieved
                    //then I need to notify the user that there is a maximum value and that is how many were retrieved
                    newJoke = await MakeRequest();
                    Jokes.Add(newJoke);
                    //put joke in correct format for logger
                    LogJoke logJoke = new(); 
                    //I am creating a new logJoke everytime with same name
                    //is it better to just create one at the top of the file?
                    logJoke.Joke = newJoke.joke;
                    logJoke.DateRequested = DateTime.UtcNow;

                    //put joke into database
                    await _logJokeRepository.Create(logJoke);
                }

                return Ok(Jokes);
            }

        }

        [HttpGet("log")]
        public async Task<ActionResult<LogJoke>> GetLoggedJoke(int id)
        {
            return await _logJokeRepository.Get(id);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<LogJoke>> GetLoggedJokes(int count = 1)
        {
            return await _logJokeRepository.GetCount(count);

        }

        [HttpPost]
        public async Task<ActionResult<LogJoke>> PostLogJokes([FromBody] Joke Joke)
        {
            LogJoke logJoke = new();
            logJoke.Joke = Joke.joke;
            logJoke.DateRequested = DateTime.UtcNow;
            //there are no checks to make sure the joke isn't just an empty string
            var newLogJoke = await _logJokeRepository.Create(logJoke);
            return CreatedAtAction(nameof(GetLoggedJoke), new { id = newLogJoke.Id }, newLogJoke);
        }


    }
}
