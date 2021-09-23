using JokeLogger.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JokeLogger.Repos.Repository
{
    public interface ILogJokeRepository
    {
        Task<IEnumerable<LogJoke>> Get();
        Task<LogJoke> Create(LogJoke logJoke);
        Task<LogJoke> Get(int id);
        Task<IEnumerable<LogJoke>> GetCount(int count);
    }
}
