using System;

namespace JokeLogger.Models.Entities
{
    public class LogJoke
    {
        public int Id { get; set; }
        public string Joke { get; set; }
        public DateTime DateRequested { get; set; }
    }
}
