using System;
using System.Collections;
using System.Collections.Generic;

namespace TSP
{
    public class SwarmSetting
    {
        public double[][] Cost { get; set; }
        public int SwarmSize { get; set; }
        public double Alpha { get; set; } = 0.98;
        public double Gamma { get; set; } = 0.98;

        public double InitialR { get; set; }
    }
    public class Swarm : IEnumerable<Bat>
    {
        private int N { get; set; }

        public List<Bat> Bats { get; set; } = new List<Bat>();

        public SwarmSetting Setting { get; set; } = null;
        public Swarm(SwarmSetting setting)
        {
            Setting = setting;
            N = setting.Cost.GetLength(0);

            for (int i = 0; i < setting.SwarmSize; i++)
            {
                var b = new Bat(0)
                {
                    A = Program.Random.NextDouble(),
                    R = Program.Random.NextDouble(),
                    V = Program.Random.Next(1, N),
                };
                Bats.Add(b);
            }
        }


        public Path BestPath { get; set; } = null;
        public double BestSolution { get; set; } = int.MaxValue;

        public int StepNumber { get; private set; } = 1;
        public bool IsSolutionCompleted { get; private set; } = false;

        public void NextStep()
        {
            if (IsSolutionCompleted)
                throw new Exception("There's no available cities to next movement");

            StepNumber++;

            Path bestPath = null;

            for (int i = 0; i < Bats.Count; i++)
            //foreach (var bat in Bats)
            {
                var bat = Bats[i];

                bat.V = Program.Random.Next(1, Helper.HammingDistance(bat.Path, bestPath));
                if (bat.V < N / 2)
                    bat.Path = Helper.TwoOpt(bat.Path);
                else
                    bat.Path = Helper.ThreeOpt(bat.Path);

                double rand = Program.Random.NextDouble();
                if (rand > bat.R)
                {
                    Bat bestBat = Bats[0];
                    for (int j = 1; j < i; j++)
                    {
                        if (Bats[j].Cost < bestBat.Cost)
                            bestBat = Bats[j];
                    }



                }
                double curSolution = Function(bat.Path);
                if (rand < bat.A && curSolution <= BestSolution)
                {
                    bestPath = bat.Path;
                    BestSolution = curSolution;

                    bat.R = Setting.InitialR * (1 - Math.Exp(-StepNumber * Setting.Gamma));
                    bat.A = Setting.Alpha * bat.A;
                }

            }

            if (StepNumber == N)
                IsSolutionCompleted = true;
        }
        double Function(Path path) => path.GetCost(Setting.Cost);

        public IEnumerator<Bat> GetEnumerator() => Bats.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
