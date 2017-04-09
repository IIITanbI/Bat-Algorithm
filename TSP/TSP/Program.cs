using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP
{
    class Program
    {
        public static Random Random { get; set; } = new Random();

        static void Main(string[] args)
        {
            int n = 20;
            int maxIteration = n*5;
            List<Bat> population = new List<Bat>();

            for (int i = 0; i < n; i++)
            {
                var b = new Bat()
                {
                    A = Random.NextDouble(),
                    R = Random.NextDouble(),
                    V = Random.NextDouble(),
                };
                population.Add(b);
            }

            for(int t = 0; t < maxIteration; t++)
            {
                for(int i = 0; i < n; i++)
                {

                }
            }

        }
    }
}
