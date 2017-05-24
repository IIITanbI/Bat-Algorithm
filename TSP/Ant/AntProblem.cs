using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ant
{
    public class AntProblem
    {
        private int n = 0;
        private int m = 0;
        private List<int> bestWay = new List<int>();
        internal double[][] trails;
        public double BestLength { get; private set; } = -1;

        public int MaxIteration { get; set; } = 500;
        public double MaxTrail { get; set; } = 400;
        public double MinTrail { get; set; } = 0.1;
        public int EliteAntCount { get; set; } = 10;
        public double Alpha { get; set; } = 0.8;
        public double Beta { get; set; } = 0.9;
        public double DefaultTrail { get; set; } = 1;
        public double Evaporation { get; set; } = 0.5;
        public double[][] Distances { get; set; }
        public double Q { get; set; } = 6500;

        


        public List<int> Solve()
        {
            n = Distances.Length;
            m = n;
            trails = CreateArray(n, n, DefaultTrail);

            for (int t = 0; t < MaxIteration; t++)
            {
                var ants = new List<Ant>();
                for (int k = 0; k < m; k++)
                    ants.Add(new Ant(this, k));

                var tasks = new List<Task>();
                for (int k = 0; k < m; k++)
                {
                    int _k = k;
                    Task task = Task.Factory.StartNew(() =>
                    {
                        for (int town = 1; town < n; town++)
                            ants[_k].GoToNext();
                        ants[_k].GoToStart();
                    }
                    );
                    tasks.Add(task);
                }
                Task.WaitAll(tasks.ToArray());
                UpdateBestResult(ants);
                UpdateTrails(ants);
            }

            return bestWay;
        }

        void UpdateTrails(List<Ant> ants)
        {
            double prev = -1;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    prev = trails[i][j];
                    trails[i][j] *= Evaporation;
                    if (trails[i][j] != 0 && trails[i][j] < 0.00001)
                    {

                    }
                }

            foreach (Ant ant in ants)
            {
                var way = ant.Way;
                double dPh = Q / ant.WayLength;
                //1 -> 2 -> 3 -> 4 -> 1
                for (int i = 0; i < way.Count - 1; i++)
                    trails[way[i]][way[i + 1]] += dPh;
            }

            {
                var way = bestWay;
                double dPh = EliteAntCount * Q / BestLength;
                //1 -> 2 -> 3 -> 4 -> 1
                for (int i = 0; i < way.Count - 1; i++)
                    trails[way[i]][way[i + 1]] += dPh;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    trails[i][j] = Math.Min(trails[i][j], MaxTrail);
                    trails[i][j] = Math.Max(trails[i][j], MinTrail);
                }

            //_test = trails.Select(t => t.Max()).Max();
        }
        void UpdateBestResult(List<Ant> ants)
        {
            foreach (Ant ant in ants)
            {
                if (!ant.IsAlive)
                {
                    Console.WriteLine("Is not alive");
                    continue;
                }
                if (ant.Way.Count != n + 1)
                    throw new Exception("Count = " + ant.Way.Count);

                if (BestLength == -1 || ant.WayLength < BestLength)
                {
                    BestLength = ant.WayLength;
                    bestWay = ant.Way;
                }
            }
            //bool[] arr = new bool[n];
            //foreach (Ant ant in ants)
            //{
            //    if (!ant.IsAlive)
            //        continue;

            //    for (int i = 0; i < n; i++)
            //    {
            //        for (int j = i; j < n; j++)
            //        {
            //            for (int k = i + 1; k < j; k++)
            //            {
            //                arr[ant.Way[k]] = true;
            //            }
            //        }
            //    }
            //}
        }
        double[][] CreateArray(int n, int m, double defaultValue = 0)
        {
            var array = new double[n][];
            for (int i = 0; i < n; i++)
                array[i] = new double[m];

            if (defaultValue != 0)
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < m; j++)
                        if (i != j)
                            array[i][j] = defaultValue;

            return array;
        }
    }
}
