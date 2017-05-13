using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepin
{
    public class AntProblem
    {
        public int max_iteration = 500;

        int n = 0;
        int m = 0;
        List<int> bestWay = new List<int>();
        public double bestLength = -1;

        public double maxTrail = 400;
        public double minTrail = 0;
        public int EliteAnt = 4;
        public double alpha = 0.8;
        public double beta = 0.9;
        public double defaultTrail = 1;
        public double evaporation = 0.5;
        public double[][] Distances;
        public double Q = 6500;
        double[][] trails;


        List<Ant> ants = new List<Ant>();
        class Ant
        {
            Random Random { get; set; } = new Random();
            static Ant()
            {
            }

            AntProblem _parent = null;
            public List<int> Way => _way;

            public bool IsAlive { get; set; } = true;
            public double Length { get; private set; } = 0;
            public int CurTown { get; private set; } = -1;
            public int StartTown { get; private set; } = -1;

            List<int> _way = new List<int>();
            bool[] _visited;

            public Ant(AntProblem parent, int startTown)
            {
                _parent = parent;
                CurTown = startTown;
                StartTown = startTown;
                _visited = new bool[_parent.Distances.Length];
                _visited[StartTown] = true;
                _way.Add(StartTown);
            }

            private void GoTo(int town)
            {
                if (town == -1)
                {
                    IsAlive = false;
                    return;
                }
                _visited[town] = true;
                _way.Add(town);
                Length += _parent.Distances[CurTown][town];
                CurTown = town;
            }
            public void GoToNext()
            {
                GoTo(NextTown());
            }
            public void GoToStart()
            {
                GoTo(StartTown);
            }

            public int NextTown()
            {
                int n = _parent.Distances.Length;

                int i = CurTown;
                double summ = 0;

                List<int> allowed = new List<int>();
                for (int j = 0; j < n; j++)
                    if (i != j && _parent.Distances[i][j] != -1 && !_visited[j])
                        allowed.Add(j);

                double prev = 0;
                foreach (int j in allowed)
                {
                    prev = summ;
                    summ += Math.Pow(_parent.trails[i][j], _parent.alpha) * Math.Pow(1.0 / _parent.Distances[i][j], _parent.beta);
                    if (double.IsInfinity(summ))
                    {

                    }
                }

                double[] prob = new double[n];
                foreach (int j in allowed)
                    prob[j] = Math.Pow(_parent.trails[i][j], _parent.alpha) * Math.Pow(1.0 / _parent.Distances[i][j], _parent.beta) / summ;

                double total = prob.ToList().Sum();
                if (1.0 - total > 0.0001)
                    Console.WriteLine("Total = " + total);

                double value = Random.NextDouble();

                double curSumm = 0;
                foreach (int j in allowed)
                {
                    curSumm += prob[j];
                    if (curSumm >= value)
                        return j;
                }

                return -1;
            }
        }

        public static double t1 = 0, t2 = 0, t3 = 0, total = 0;
        public List<int> Solve()
        {
            n = Distances.Length;
            m = n;
            trails = CreateArray(n, n, defaultTrail);

            var sw1 = Stopwatch.StartNew();
           
            for (int t = 0; t < max_iteration; t++)
            {
                var sw = new Stopwatch();
                ants = new List<Ant>();
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
                // var _t1 = sw.ElapsedMilliseconds;
                UpdateBest();
                //var _t2 = sw.ElapsedMilliseconds - _t1;
                UpdateTrails();
                //var _t3 = sw.ElapsedMilliseconds;

                //t1 += _t1 / 1000.0;
                //t2 += _t2 / 1000.0;
                // sw.Stop();
                t3 += sw.ElapsedMilliseconds;
                //sw1.Stop();
                //sw1.Restart();
            }
            total += sw1.ElapsedMilliseconds;
            return bestWay;
        }

        public static double _test { get; set; } = 0;
        void UpdateTrails()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    trails[i][j] *= evaporation;

            foreach (Ant ant in ants)
            {
                var way = ant.Way;
                double dPh = Q / ant.Length;
                //1 -> 2 -> 3 -> 4 -> 1
                for (int i = 0; i < way.Count - 1; i++)
                    trails[way[i]][way[i + 1]] += dPh;
            }

            {
                var way = bestWay;
                double dPh = EliteAnt * Q / bestLength;
                //1 -> 2 -> 3 -> 4 -> 1
                for (int i = 0; i < way.Count - 1; i++)
                    trails[way[i]][way[i + 1]] += dPh;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    trails[i][j] = Math.Min(trails[i][j], maxTrail);
                    trails[i][j] = Math.Max(trails[i][j], minTrail);
                }

            //_test = trails.Select(t => t.Max()).Max();
        }
        void UpdateBest()
        {
            foreach (Ant ant in ants)
            {
                if (!ant.IsAlive)
                {
                    Console.WriteLine("is not alive");
                    continue;
                }
                if (ant.Way.Count != n + 1)
                    Console.WriteLine("Count = " + ant.Way.Count);

                if (bestLength == -1 || ant.Length < bestLength)
                {
                    bestLength = ant.Length;
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
        static double[][] CreateArray(int n, int m, double defaultValue = 0)
        {
            var array = new double[n][];
            for (int i = 0; i < n; i++)
                array[i] = new double[m];

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (i != j)
                        array[i][j] = defaultValue;

            return array;
        }
    }
}
