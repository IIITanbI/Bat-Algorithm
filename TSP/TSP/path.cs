using System.Collections.Generic;

namespace TSP
{
    public class Path
    {
        public List<int> Cities { get; set; } = new List<int>();

        public Path() { }
        public Path(Path path) => Cities.AddRange(path.Cities);
        public Path(List<int> cities) => Cities.AddRange(cities);

        public double GetCost(double[][] cost)
        {
            double res = 0;
            for (int i = 0; i < Cities.Count - 1; i++)
            {
                res += cost[Cities[i]][Cities[i + 1]];
            }
            return res;
        }
    }
}
