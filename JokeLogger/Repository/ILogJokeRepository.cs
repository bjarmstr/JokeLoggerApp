using JokeLogger.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JokeLogger.Repository
{
    public interface ILogJokeRepository
    {
        Task<IEnumerable<LogJoke>> Get();
        Task<LogJoke> Create(LogJoke logJoke);
        Task<LogJoke> Get(int id);
        Task<IEnumerable<LogJoke>> GetCount(int count);
    }
}
