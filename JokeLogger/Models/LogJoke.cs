using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeLogger.Models
{
    public class LogJoke
    {
        public int Id { get; set; }
        public string Joke { get; set; }
        public DateTime DateRequested { get; set; }
    }
}
