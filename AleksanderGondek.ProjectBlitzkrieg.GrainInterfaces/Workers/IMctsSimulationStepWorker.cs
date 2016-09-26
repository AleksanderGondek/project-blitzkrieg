using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers
{
    public interface IMctsSimulationStepWorker : IGrainWithGuidKey
    {
        Task<bool> PerformSimulation(ProcessingRequest request);
    }
}