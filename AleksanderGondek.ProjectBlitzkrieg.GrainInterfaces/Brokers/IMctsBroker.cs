using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers
{
    public interface IMctsBroker : IGrainWithGuidKey
    {
        Task<string> GetNextMove(ProcessingRequest request);
    }
}