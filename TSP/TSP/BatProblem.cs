using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    public class BatProblem
    {
        public static Random Random { get; set; } = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

        public int MaxIteration = 10;
        public int SwarmSize = 50;
        public double[][] Cost;
        public List<int> Solve()
        {
            int n = Cost.Length;

            Path bestPath = new Path();
            double bestSolution = double.MaxValue;

            var swarmSetting = new SwarmSetting()
            {
                CostMatrix = Cost,
                SwarmSize = SwarmSize,
                InitialR = Random.NextDouble() % 0.41
            };

            for (int t = 0; t < MaxIteration; t++)
            {
                Swarm swarm = new Swarm(swarmSetting);
                for (int tt = 0; tt < n; tt++)
                {
                    if (tt == n - 1)
                    {
                        var tttt = swarm.BestPath.Cities.Distinct().ToList();
                        var dd = swarm.BestPath.Cities.Where(i => swarm.BestPath.Cities.Count(ii => ii == i) > 1).ToList();
                    }
                    swarm.NextStep();
                }

                if (swarm.BestSolution < bestSolution)
                {
                    bestSolution = swarm.BestSolution;
                    bestPath = swarm.BestPath;
                }
            }

            return bestPath.Cities;
        }
    }
}
