using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JokeLogger.Models
{
    public class Joke
    {
       // public long Id { get; set; }
        public string joke { get; set; }
    }
}
