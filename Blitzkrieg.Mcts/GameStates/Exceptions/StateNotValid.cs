using System;
using System.Collections.Generic;

namespace Blitzkrieg.Mcts.GameStates.Exceptions
{
    public class StateNotValid : Exception
    {
        public StateNotValid(string errorMessage) : base(errorMessage){}
        public IList<string> ValidationIssues;
    }
}
