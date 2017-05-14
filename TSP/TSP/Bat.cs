using System.Linq;

namespace TSP
{
    public class Bat
    {
        /// <summary>
        /// Pulse rate
        /// </summary>
        public double R { get; set; }
        /// <summary>
        /// Velocity
        /// </summary>
        public int V { get; set; }
        /// <summary>
        /// Loudness
        /// </summary>
        public double A { get; set; }


        public int CurrentCity { get; private set; }
        public int StartCity { get; private set; }


        private Path _path = new Path();
        public Path Path
        {
            get
            {

                return _path;
            }
            private set
            {
                _path = value;
            }
        }
        public double Cost { get; set; } = 0;

        public bool[] Used { get; private set; }

        public Bat(int startCity, int n)
        {
            CurrentCity = StartCity = startCity;
            Path.Cities.Add(startCity);
            Used = new bool[n];
            Used[CurrentCity] = true;
        }

        public void SetPath(Path path)
        {
            for (int i = 0; i < Used.Length; i++)
                Used[i] = false;
            foreach (var i in path.Cities)
                Used[i] = true;
            Path = path;
        }

        public void NextCity(double[][] costMatrix, int neighbourCount = -1)
        {
            if (CurrentCity == -1)
                throw new System.Exception($"CurrentCity = -1");

            int n = costMatrix.GetLength(0);

            if (Path.Cities.Count == n)
            {
                //go to start
                Used[StartCity] = false;
            }
            bool examineAll = neighbourCount == -1;
            int examine = neighbourCount;

            Path bestPath = null;

            for (int i = 0; i < n; i++)
            {
                int from = CurrentCity;
                int to = i;
                if (Used[to])
                    continue;
                double c = costMatrix[from][to];
                if (c == -1)
                    continue;

                var newPath = new Path(Path);
                newPath.Cities.Add(to);

                var tPath = newPath.BestPath(costMatrix);

                if (bestPath == null || tPath.GetCost(costMatrix) < bestPath.GetCost(costMatrix))
                {
                    //if (tPath.Cities.First() != tPath.Cities.Last() &&   tPath.Cities.Distinct().Count() != tPath.Cities.Count)
                    //{
                    //    throw new System.Exception("Err");
                    //}
                    bestPath = tPath;
                }

                examine++;
                if (!examineAll && examine == V)
                    break;
            }
            if (bestPath.Cities.Distinct().Count() != n)
            {
                //  throw new System.Exception("Err");
            }

            Path = bestPath;
            Cost = Path.GetCost(costMatrix);

            CurrentCity = Path.Cities.Last();
            Used[CurrentCity] = true;

            foreach (var i in _path.Cities)
            {
                if (!Used[i])
                {
                    throw new System.Exception("Err");
                }
            }
        }
    }
}
