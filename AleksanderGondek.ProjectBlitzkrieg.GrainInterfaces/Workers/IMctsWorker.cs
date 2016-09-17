using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers
{
    public interface IMctsWorker : IGrainWithGuidKey
    {
        Task<string> GetNextMove(ProcessingRequest request);
    }
}