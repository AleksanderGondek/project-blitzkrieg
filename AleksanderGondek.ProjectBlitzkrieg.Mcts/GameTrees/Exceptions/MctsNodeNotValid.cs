using System;
using System.Collections.Generic;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Exceptions
{
    public class MctsNodeNotValid : Exception
    {
        public MctsNodeNotValid(string errorMessage) : base(errorMessage){ }
        public IList<string> ValidationIssues;
    }
}
