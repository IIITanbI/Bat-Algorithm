using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace TSP
{
    public class Swarm 
    {
        private int N { get; set; }

        public List<Bat> Bats { get; set; } = new List<Bat>();

        public SwarmSetting Setting { get; set; } = null;
        public Swarm(SwarmSetting setting)
        {
            Setting = setting;
            N = setting.CostMatrix.GetLength(0);

            for (int i = 0; i < setting.SwarmSize; i++)
            {
                var b = new Bat(0, N)
                {
                    A = (double)BatProblem.Random.Next(700, 1000) / 1000.0,
                    R = BatProblem.Random.NextDouble(),
                    V = 1,
                };
                Bats.Add(b);
            }
            BestPath = new Path();
            BestPath.Cities.Add(0);
        }


        public Path BestPath { get; set; } = null;
        public double BestSolution { get; set; } = double.MaxValue;

        public int StepNumber { get; private set; } = 1;
        public bool IsSolutionCompleted { get; private set; } = false;

        public static double timer = 0;
        public void NextStep()
        {
            var sw = Stopwatch.StartNew();
            if (IsSolutionCompleted)
                throw new Exception("There's no available cities to next movement");

            StepNumber++;

            Path bestPath = null;
            double bestSolution = double.MaxValue;


            for (int i = 0; i < Bats.Count; i++)
            //foreach (var bat in Bats)
            {
                var bat = Bats[i];
                var tPath = new Path(bat.Path);
                bat.NextCity(Setting.CostMatrix, bat.V);

                int dst = tPath.HammingDistance(BestPath);
                dst = Math.Max(dst, 2);
                bat.V = BatProblem.Random.Next(1, dst);
                bat.SetPath(bat.Path.TwoOpt());

                double rand = BatProblem.Random.NextDouble();
                if (rand > bat.R)
                {
                    Bat bestBat = Bats[0];
                    for (int j = 1; j < i; j++)
                    {
                        if (Bats[j].Cost < bestBat.Cost)
                            bestBat = Bats[j];

                        var tempPath = bestBat.Path.BestPath(Setting.CostMatrix);
                        bat.SetPath(tempPath);
                        bat.Cost = bat.Path.GetCost(Setting.CostMatrix);
                    }
                }
                double curSolution = Function(bat.Path);
                if (bestPath == null || curSolution <= bestSolution)
                {
                    bestPath = bat.Path;
                    bestSolution = curSolution;

                    bat.R = Setting.InitialR * (1 - Math.Exp(-StepNumber * Setting.Gamma));
                    bat.A = Setting.Alpha * bat.A;
                }
            }

            BestPath = bestPath;
            BestSolution = bestSolution;

            if (StepNumber == N + 1)
                IsSolutionCompleted = true;
            timer += sw.ElapsedMilliseconds / 1000.0;
        }
        double Function(Path path) => path.GetCost(Setting.CostMatrix);
    }
}
