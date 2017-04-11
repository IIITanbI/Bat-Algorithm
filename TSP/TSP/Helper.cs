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
            return new Path(path);
        }
        public static Path ThreeOpt(Path path)
        {
            return new Path(path);
        }
    }
}
