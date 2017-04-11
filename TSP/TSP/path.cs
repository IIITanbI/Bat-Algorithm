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

        //public void Add(int city)
        //{
        //    Cities.Add(city);
        //    IsChanged = true;
        //}
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
    }
}
