using System;

namespace AleksanderGondek.ProjectBlitzkrieg.Mcts.GameTrees.Utilities
{
    public static class NodeComparators
    {
        public static int CompareWithBasicUcb(IMctsNode nodeOne, IMctsNode nodeTwo)
        {
            var nodeOneValueAsDouble = Convert.ToDouble(nodeOne.Value);
            var nodeTwoValueAsDouble = Convert.ToDouble(nodeTwo.Value);
            var nodeOneVisitsAsDouble = Convert.ToDouble(nodeOne.Visits);
            var nodeTwoVisitsAsDouble = Convert.ToDouble(nodeTwo.Visits);

            var nodeOneValue = (nodeOneValueAsDouble / nodeOneVisitsAsDouble) + Math.Sqrt(2*Math.Log(nodeTwoVisitsAsDouble/nodeOneVisitsAsDouble));
            var nodeTwoValue = (nodeTwoValueAsDouble / nodeTwoVisitsAsDouble) + Math.Sqrt(2 * Math.Log(nodeOneVisitsAsDouble /nodeTwoVisitsAsDouble));

            if (nodeOneValue > nodeTwoValue)
            {
                return -1;
            }
            return nodeTwoValue > nodeOneValue ? 1 : 0;
        }
    }
}
