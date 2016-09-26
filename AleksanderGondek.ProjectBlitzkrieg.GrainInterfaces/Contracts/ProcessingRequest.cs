using System;

namespace AleksanderGondek.ProjectBlitzkrieg.GrainInterfaces.Contracts
{
    [Serializable]
    public class ProcessingRequest
    {
        public string GameState;
        public string ExectutionType;
        public int Workers;
        public int MaximumIterations;
        public int MaxiumumSimulations;
        public Guid TargetGrain;
    }
}
