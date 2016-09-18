using System.Collections.Generic;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers
{
    public interface IMctsWorker : IGrainWithGuidKey
    {
        Task<IDictionary<string, int>> GetNextMove(ProcessingRequest request);
    }
}