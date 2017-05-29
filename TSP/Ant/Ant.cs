using System;
using System.Collections.Generic;
using System.Linq;

namespace Ant
{
    public class Ant
    {
        static Random Random { get; set; } = new Random((int)(DateTime.Now.Ticks % int.MaxValue));

        private AntProblem _parent { get; set; }
        private List<int> _way = new List<int>();
        private bool[] _visited;

        public List<int> Way => _way;
        public double WayLength { get; private set; } = 0;

        public bool IsAlive { get; private set; } = true;
        public int CurrentCity { get; private set; } = -1;
        public int StartCity { get; private set; } = -1;


        public Ant(AntProblem parent, int startTown)
        {
            _parent = parent;
            CurrentCity = startTown;
            StartCity = startTown;

            _visited = new bool[_parent.Distances.Length];
            _visited[StartCity] = true;
            _way.Add(StartCity);
        }

        public void GoToNext()
        {
            GoTo(GetNextCity());
        }
        public void GoToStart()
        {
            GoTo(StartCity);
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
            WayLength += _parent.Distances[CurrentCity][town];
            CurrentCity = town;
        }
        private int GetNextCity()
        {
            int n = _parent.Distances.Length;

            int i = CurrentCity;
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
                    throw new Exception("Infinity");
                if (double.IsNaN(summ))
                    throw new Exception("NaN");
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
}
