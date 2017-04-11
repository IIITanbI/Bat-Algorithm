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
            int n = 20;
            Cost = new double[n][];
            for (int i = 0; i < n; i++)
                Cost[i] = new double[n];

            int solveIteration = 10;
            List<Bat> population = new List<Bat>();


            Path bestPath = new Path();
            double bestSolution = double.MaxValue;

            var swarmSetting = new SwarmSetting()
            {
                SwarmSize = 50,
                Cost = Cost,
                Alpha = 0.98,
                Gamma = 0.98
            };

            Swarm swarm = new Swarm(swarmSetting);
            for (int t = 0; t < solveIteration; t++)
            {
                for (int tt = 0; tt < n; tt++)
                {
                    swarm.NextStep();
                }

                if (swarm.BestSolution < bestSolution)
                {
                    bestSolution = swarm.BestSolution;
                    bestPath = swarm.BestPath;
                }
            }
        }


        double Function(Path path)
        {
            return path.GetCost(Cost);
        }
    }
}
