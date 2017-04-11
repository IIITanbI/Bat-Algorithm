using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{

    class Program
    {
        public static Random Random { get; set; } = new Random();

        static void Main(string[] args)
        {
            new Program().Run();
        }

        double[][] Cost;
        void Run()
        {
            int n = 8;
            Cost = new double[n][];
            for (int i = 0; i < n; i++)
            {
                Cost[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    Cost[i][j] = Random.Next(1, 100);
                }
            }

            int solveIteration = 10;

            Path bestPath = new Path();
            double bestSolution = double.MaxValue;

            var swarmSetting = new SwarmSetting()
            {
                CostMatrix = Cost,
                SwarmSize = 50,
                InitialR = Random.NextDouble() % 0.41
            };

            for (int t = 0; t < solveIteration; t++)
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


            var trueSolve = Stepin.Program.StupidSolve(Cost);

            double res2 = 0;
            for (int i = 0; i < trueSolve.Count - 1; i++) res2 += Cost[trueSolve[i]][trueSolve[i + 1]];

            Console.WriteLine(bestSolution);
            Console.WriteLine(res2);
        }
    }
}
