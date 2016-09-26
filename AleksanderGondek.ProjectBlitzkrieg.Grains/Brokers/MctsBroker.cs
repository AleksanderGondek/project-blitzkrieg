using System;
using System.Linq;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Brokers;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.Persistence.Repositories;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Brokers
{
    public class MctsBroker : Grain, IMctsBroker
    {
        public async Task<string> GetNextMove(ProcessingRequest request)
        {
            IMctsWorker worker;
            if (AvailableExecutionTypes.MctsSerialWithUtc.Equals(request.ExectutionType))
            {
                worker = GrainFactory.GetGrain<IMctsSerialWithUtcWorker>(request.TargetGrain);
            }
            else if (AvailableExecutionTypes.MctsRootParallelizationWithUct.Equals(request.ExectutionType))
            {
                worker = GrainFactory.GetGrain<IMctsRootParallelizationWithUtcWorker>(Guid.NewGuid());
            }
            else if (AvailableExecutionTypes.MctsLeafParallelizationWithUct.Equals(request.ExectutionType))
            {
                worker = GrainFactory.GetGrain<IMctsLeafParallelizationWithUtcWorker>(Guid.NewGuid());
            }
            else if (AvailableExecutionTypes.MctsSharedTreeParallelizationWithUtc.Equals(request.ExectutionType))
            {
                worker = GrainFactory.GetGrain<IMctsSharedTreeWithUctMasterWorker>(Guid.NewGuid());
            }
            else
            {
                return string.Empty;
            }
            
            var possibleMovesWithScores = await worker.GetNextMove(request);
            var action = possibleMovesWithScores.First(x => x.Value == possibleMovesWithScores.Values.Max()).Key;
            return action;
        }

        public Task<bool> CleanInMemoryRepository()
        {
            var inMemoryRepository = new InMemoryDocumentRepository<MctsNode>();
            inMemoryRepository.CleanAll();
            return Task.FromResult(true);
        }
    }
}