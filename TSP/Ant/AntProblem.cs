using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ant
{
    public class AntProblem
    {
        int n = 0;
        int m = 0;
        List<int> bestWay = new List<int>();
        public double BestLength { get; set; } = -1;

        public int MaxIteration { get; set; } = 500;
        public double MaxTrail { get; set; } = 400;
        public double MinTrail { get; set; } = 0;
        public int EliteAnt { get; set; } = 4;
        public double Alpha { get; set; } = 0.8;
        public double Beta { get; set; } = 0.9;
        public double DefaultTrail { get; set; } = 1;
        public double Evaporation { get; set; } = 0.5;
        public double[][] Distances { get; set; }
        public double Q { get; set; } = 6500;
        double[][] trails;

        List<Ant> ants = new List<Ant>();
        public class Ant
        {
            static Random Random { get; set; } = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

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
                    summ += Math.Pow(_parent.trails[i][j], _parent.Alpha) * Math.Pow(1.0 / _parent.Distances[i][j], _parent.Beta);
                    if (double.IsInfinity(summ))
                    {
                        throw new Exception("Infinity");
                    }
                }

                double[] prob = new double[n];
                foreach (int j in allowed)
                    prob[j] = Math.Pow(_parent.trails[i][j], _parent.Alpha) * Math.Pow(1.0 / _parent.Distances[i][j], _parent.Beta) / summ;

                double total = prob.ToList().Sum();
                if (1.0 - total > 0.0001)
                    throw new Exception("Total = " + total);

                double value = 0;
                lock (Random)
                    value = Random.NextDouble();

                double curSumm = 0;
                foreach (int j in allowed)
                {
                    curSumm += prob[j];
                    if (curSumm >= value)
                        return j;
                }

                throw new Exception("Next town is -1");
                //return -1;
            }
        }

        public List<int> Solve()
        {
            n = Distances.Length;
            m = n;
            trails = CreateArray(n, n, DefaultTrail);

            for (int t = 0; t < MaxIteration; t++)
            {
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
                UpdateBest();
                UpdateTrails();
            }

            return bestWay;
        }

        void UpdateTrails()
        {
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    trails[i][j] *= Evaporation;

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
                double dPh = EliteAnt * Q / BestLength;
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
        void UpdateBest()
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

                if (BestLength == -1 || ant.Length < BestLength)
                {
                    BestLength = ant.Length;
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

            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    if (i != j)
                        array[i][j] = defaultValue;

            return array;
        }
    }
}
