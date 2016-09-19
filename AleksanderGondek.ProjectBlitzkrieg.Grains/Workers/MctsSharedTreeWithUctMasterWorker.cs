using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts;
using AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Workers;
using Orleans;

namespace AleksanderGondek.ProjectBlitzkrieg.Grains.Workers
{
    public class MctsSharedTreeWithUctMasterWorker: Grain, IMctsSharedTreeWithUctMasterWorker
    {
        public async Task<IDictionary<string, int>> GetNextMove(ProcessingRequest request)
        {
            var workersTasks = new List<Task<IDictionary<string, int>>>();
            for (var i = 0; i < request.Workers; i++)
            {
                var worker = GrainFactory.GetGrain<IMctsSharedTreeWithUctWorker>(Guid.NewGuid());
                workersTasks.Add(worker.GetNextMove(request));
            }
            var allTasks = Task.WhenAll(workersTasks);
            await allTasks;

            var actions = new Dictionary<string, int>();
            foreach (var result in workersTasks)
            {
                result.Result.ToList().ForEach(x => actions[x.Key] = actions.ContainsKey(x.Key) ? actions[x.Key] + x.Value : x.Value);
            }

            //TODO : Should release used entities within document store

            return actions;
        }
    }
}
