using System;
using Xunit;
using AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.Tests
{
    public class Class1 : IDisposable
    {
        private IMctsNode _mctsNode;

        public Class1()
        {
            _mctsNode = new MctsNode();
        }

        public void Dispose()
        {
            _mctsNode = null;
        }

        [Fact]
        public void PassingTest()
        {
            Assert.True(_mctsNode != null);
        }

    }
}