using System;
using System.Collections.Generic;

namespace TSP
{
    public static class Helper
    {
        public static int HammingDistance(Path p1, Path p2)
        {
            List<int> t1 = p1.Cities;
            List<int> t2 = p2.Cities;

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

        public static Path TwoOpt(Path path)
        {
            int n = path.Cities.Count;
            if (n < 4)
                return new Path(path);

            int i, k;

            while (true)
            {
                i = Program.Random.Next(1, n - 1);
                k = Program.Random.Next(1, n - 1);
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

            var p = new Path(path);
            p.Cities.Reverse(i, k - i + 1);

            return p;
        }
        //public static Path ThreeOpt(Path path)
        //{
        //    return new Path(path);
        //}

        public static Path BestPath(double[][] costMatrix, Path path)
        {
            var newPath = new Path(path);
            double tempCost1 = newPath.GetCost(costMatrix);

            var modifiedPath = Helper.TwoOpt(newPath);
            double tempCost2 = modifiedPath.GetCost(costMatrix);

            if (tempCost1 < tempCost2)
                return newPath;
            else
                return modifiedPath;
        }
    }
}
