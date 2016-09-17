using System;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Brokers
{
    public class MctsBroker : Grain, IMctsBroker
    {
        public Task<string> GetNextMove(ProcessingRequest request)
        {
            var serialWorker = GrainFactory.GetGrain<IMctsWorker>(Guid.NewGuid());
            return serialWorker.GetNextMove(request);
        }
    }
}