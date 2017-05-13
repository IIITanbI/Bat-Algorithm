using System;
using System.Collections;
using System.Collections.Generic;

namespace TSP
{
    public class Path  
    {
        public List<int> Cities { get; set; } = new List<int>();
        //private int _cost = 0;
        //public bool IsChanged { get; private set; } = false;

        public Path() { }
        public Path(Path path)
        {
            Cities = new List<int>(path.Cities);
            //_cost = path._cost;
        }

        public double GetCost(double[][] cost)
        {
            //if (IsChanged == false)
            //    return _cost;

            double res = 0;
            for (int i = 0; i < Cities.Count - 1; i++)
            {
                res += cost[Cities[i]][Cities[i + 1]];
            }
            return res;
        }

        public int HammingDistance(Path path)
        {
            List<int> t1 = Cities;
            List<int> t2 = path.Cities;

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

        public Path TwoOpt()
        {
            int n = Cities.Count;
            if (n < 4)
                return new Path(this);

            int i, k;

            while (true)
            {
                i = TSP_Solver.Random.Next(1, n - 1);
                k = TSP_Solver.Random.Next(1, n - 1);
                if (i == k)
                    continue;

                if (i > k)
                {
                    int temp = i;
                    i = k;
                    k = i;
                }
                break;
            }

            var p = new Path(this);
            p.Cities.Reverse(i, k - i + 1);

            return p;
        }

        public Path BestPath(double[][] costMatrix)
        {
            var newPath = new Path(this);
            double tempCost1 = newPath.GetCost(costMatrix);

            var modifiedPath = TwoOpt();
            double tempCost2 = modifiedPath.GetCost(costMatrix);

            if (tempCost1 < tempCost2)
                return newPath;
            else
                return modifiedPath;
        }
    }
}
