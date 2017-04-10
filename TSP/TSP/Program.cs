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


            int maxIteration = n * 5;
            List<Bat> population = new List<Bat>();
            Path bestPath = new Path();
            int bestSolution = int.MaxValue;

            for (int i = 0; i < n; i++)
            {
                var b = new Bat()
                {
                    A = Random.NextDouble(),
                    R = Random.NextDouble(),
                    V = Random.NextDouble(),
                };
                population.Add(b);
            }

            for (int t = 0; t < maxIteration; t++)
            {
                for (int i = 0; i < n; i++)
                {
                    var bat = population[i];
                    bat.V = Random.Next(1, HammingDistance(bat.Path, bestPath));
                    if (bat.V < n / 2)
                        bat.Path = TwoOpt(bat.Path);
                    else
                        bat.Path = ThreeOpt(bat.Path);

                    double rand = Random.NextDouble();
                    if (rand > bat.R)
                    {

                    }
                    if (rand < bat.A && Function(bat.Path) < bestSolution)
                    {
                        bestPath = bat.Path;
                    }
                }


            }
        }


        int HammingDistance(Path p1, Path p2) => HammingDistance(p1.Cities, p2.Cities);
        int HammingDistance(List<int> t1, List<int> t2)
        {
            if (t1.Count != t2.Count)
                throw new NotImplementedException();

            int res = 0;
            for (int i = 0; i < t1.Count; i++)
            {
                if (t1[i] != t2[i])
                    res++;
            }
            return res;
        }

        Path TwoOpt(Path path)
        {
            return new Path(path);
        }
        Path ThreeOpt(Path path)
        {
            return new Path(path);
        }

        double Function(Path path)
        {
            return path.GetCost(Cost);
        }
    }
}
