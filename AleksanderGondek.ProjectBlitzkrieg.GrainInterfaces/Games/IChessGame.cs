using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Games
{
    public interface IChessGame
    {
        Task<string> StartGame(ProcessingRequest request);
        Task<string> GetGameStateJson(string gameId);
    }
}
