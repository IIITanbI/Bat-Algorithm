using Ant;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP;

namespace Runner
{
    public class Program
    {
        static void PrintMatrix(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    var value = Math.Round(matrix[i][j], 2);
                    Console.Write(value.ToString().PadLeft(6));
                }
                Console.WriteLine();
            }
        }
        static void PrintMatrixInFile(double[][] matrix, string file)
        {
            StringBuilder build = new StringBuilder();
            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix.Length; j++)
                {
                    var value = Math.Round(matrix[i][j], 2);
                    build.Append(value.ToString().PadLeft(6));
                }
                build.AppendLine();
            }
            string str = build.ToString();
            File.WriteAllText(file, str);
        }
        public static double[][] RandomDst(int n)
        {
            Random random = new Random();
            double[][] distances = new double[n][];
            for (int i = 0; i < n; i++)
                distances[i] = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    var dst = random.Next(10, 20);
                    distances[i][j] = dst;
                    distances[j][i] = dst;
                }
            }
            return distances;
        }

        static double RunAnt(double[][] distances)
        {
            Console.WriteLine("Ant Problem");

            List<double> solves = new List<double>();
            Dictionary<int, double> solvesIter = new Dictionary<int, double>();
            List<KeyValuePair<double, Tuple<double, double>>> abd = new List<KeyValuePair<double, Tuple<double, double>>>();
            List<int> elites = new List<int>() { 0, 1, 2, 3, 4, 5, 10, 15, 20, 25 };
            elites = new List<int>() { 10 };

            List<int> iterations = new List<int>() { 1000 };
            Stopwatch global = Stopwatch.StartNew();
            for (int it = 0; it < iterations.Count; it++)
            {
                int _it = iterations[it];
                solvesIter.Add(_it, double.MaxValue);
                for (int j = 0; j < elites.Count; j++)
                {
                    for (double a = 0.8; a <= 1; a += 0.1)
                    {
                        for (double b = 0.8; b <= 1; b += 0.1)
                        {
                            var antProblem = new AntProblem()
                            {
                                Alpha = a,
                                Beta = b,
                                EliteAnt = elites[j],
                                Distances = distances,
                                MaxIteration = _it
                            };
                            var antSolve = antProblem.Solve();

                            double res = 0;
                            for (int i = 0; i < antSolve.Count - 1; i++)
                                res += distances[antSolve[i]][antSolve[i + 1]];

                            solves.Add(res);
                            Console.WriteLine(solves.Min());
                            solvesIter[_it] = Math.Min(res, solvesIter[_it]);
                            abd.Add(new KeyValuePair<double, Tuple<double, double>>(res, new Tuple<double, double>(a, b)));
                        }
                    }
                }
            }

            global.Stop();
            Console.WriteLine("Total Elapsed: " + global.ElapsedMilliseconds / 1000.0);
            solves.Sort();
            Console.WriteLine("All solves:\n" + string.Join(" ", solves));

            Console.WriteLine();
            Console.WriteLine("Results");
            foreach (var pair in solvesIter)
            {
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }

            Console.WriteLine();
            Console.WriteLine("Results by a b");
            abd.Sort((a, b) =>
            {
                int cmp = a.Key.CompareTo(b.Key);
                if (cmp == 0)
                {
                    int cmp1 = a.Value.Item1.CompareTo(b.Value.Item1);
                    if (cmp1 == 0)
                        return a.Value.Item2.CompareTo(b.Value.Item2);
                    else
                        return cmp1;
                }
                else
                    return cmp;
            });
            foreach (var pair in abd)
            {
                Console.WriteLine(pair.Key + " " + pair.Value);
            }
            Console.WriteLine();

            return solves.First();
        }
        static double RunBat(double[][] cost)
        {
            Console.WriteLine("Bat Problem");

            List<double> solves = new List<double>();
            Dictionary<int, double> solvesIter = new Dictionary<int, double>();

            List<int> iterations = new List<int>() { 100 };
            List<int> swarmSize = new List<int>() {30 };
            Stopwatch global = Stopwatch.StartNew();
            for (int it = 0; it < iterations.Count; it++)
            {
                int _it = iterations[it];
                solvesIter.Add(_it, double.MaxValue);
                for (int ss = 0; ss < swarmSize.Count; ss++)
                {
                    var problem = new BatProblem()
                    {
                        Cost = cost,
                        MaxIteration = _it,
                        SwarmSize = swarmSize[ss]
                    };
                    var solve = problem.Solve();

                    double res = 0;
                    for (int i = 0; i < solve.Count - 1; i++)
                        res += cost[solve[i]][solve[i + 1]];

                    solves.Add(res);
                    solvesIter[_it] = Math.Min(res, solvesIter[_it]);
                }
            }

            global.Stop();
            Console.WriteLine("Total Elapsed: " + global.ElapsedMilliseconds / 1000.0);
            solves.Sort();
            Console.WriteLine("All solves:\n" + string.Join(" ", solves));

            Console.WriteLine();
            Console.WriteLine("Results");
            foreach (var pair in solvesIter)
            {
                Console.WriteLine(pair.Key + ": " + pair.Value);
            }
            return solves.First();
        }

        static double[][] CopyArr(double[][] arr)
        {
            double[][] res = new double[arr.Length][];
            for(int i = 0; i < arr.Length; i++)
            {
                res[i] = new double[arr.Length];
                arr[i].CopyTo(res[i], 0);
            }

            return res;
        }
        static void Main(string[] args)
        {
            string folder = @"ALL_tsp";
            string file = "eil51";
            var distances = ParseDistances(System.IO.Path.Combine(folder, file + ".tsp"));
            var expectedSolve = ParseSolve(System.IO.Path.Combine(folder, file + ".opt.tour"));
            double expected = 0;
            for (int i = 0; i < expectedSolve.Count - 1; i++) expected += distances[expectedSolve[i]][expectedSolve[i + 1]];
            Console.WriteLine("Expected Result:");
            Console.WriteLine(expected);

            Console.WriteLine();
            Console.WriteLine("Start");

            double antRes = double.MaxValue;
            double batRes = double.MaxValue;
            for (int i = 0; i < 10; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Try #" + i);
                Console.ForegroundColor = ConsoleColor.Gray;

                var ar = RunAnt(CopyArr(distances));
                antRes = Math.Min(antRes, ar);
                Console.WriteLine();
                Console.WriteLine();
                //var br = RunBat(CopyArr(distances));
                //batRes = Math.Min(batRes, br);
                //Console.WriteLine();
            }

            Console.WriteLine("Best ant: " + antRes);
            Console.WriteLine("Best bat: " + batRes);
        }

        public static List<int> StupidSolve(double[][] distances)
        {
            int n = distances.Length;
            List<int> way = new List<int>();
            for (int i = 1; i < n; i++)
                way.Add(i);

            var permutes = way.Permute().Select(ie => ie.ToList()).ToList();

            double bestLength = double.MaxValue;
            List<int> bestWay = new List<int>();
            foreach (var permute in permutes)
            {
                double temp = 0;
                temp += distances[0][permute[0]];
                for (int i = 0; i < permute.Count - 1; i++)
                    temp += distances[permute[i]][permute[i + 1]];
                temp += distances[permute.Last()][0];

                if (temp < bestLength)
                {
                    bestLength = temp;
                    bestWay = permute;
                }
            }
            List<int> res = new List<int>
            {
                0
            };
            res.AddRange(bestWay);
            res.Add(0);
            return res;
        }

        static double[][] ParseDistances(string file)
        {
            var allLines = File.ReadAllLines(file);
            var lines = allLines.SkipWhile(s => s != "NODE_COORD_SECTION").Skip(1).TakeWhile(s => s != "EOF").ToList();

            double[][] res = new double[lines.Count][];
            List<Tuple<double, double>> points = new List<Tuple<double, double>>();
            for (int i = 0; i < lines.Count; i++)
            {
                var split = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                points.Add(new Tuple<double, double>(double.Parse(split[1]), double.Parse(split[2])));
            }

            points.Sort();
            var tt = points.SelectMany(p => points.Where(p1 => p.Item1 == p1.Item1 && p.Item2 == p1.Item2).Skip(1)).ToList();
            if (tt.Count > 0)
            {

            }
            for (int i = 0; i < points.Count; i++)
            {
                res[i] = new double[points.Count];
                for (int j = 0; j < points.Count; j++)
                {
                    res[i][j] = (int)(Math.Sqrt(Math.Pow(points[i].Item1 - points[j].Item1, 2) + Math.Pow(points[i].Item2 - points[j].Item2, 2)) + 0.5);
                }
            }
            return res;
        }
        static double[][] ParseDistances1(string file)
        {
            var allLines = File.ReadAllLines(file);
            var lines = allLines.SkipWhile(s => s != "EDGE_WEIGHT_SECTION").Skip(1).TakeWhile(s => s != "EOF" && s != "DISPLAY_DATA_SECTION").ToList();

            double[][] res = new double[lines.Count][];
            List<Tuple<double, double>> points = new List<Tuple<double, double>>();
            for (int i = 0; i < lines.Count; i++)
            {
                var split = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                res[i] = new double[lines.Count];
                for (int j = 0; j < lines.Count; j++)
                {
                    res[i][j] = double.Parse(split[j]);
                }
            }

            return res;
        }
        static List<int> ParseSolve(string file)
        {
            var allLines = File.ReadAllLines(file);
            var lines = allLines.SkipWhile(s => s != "TOUR_SECTION").Skip(1).TakeWhile(s => s != "-1").ToList();
            lines.Add(lines[0]);
            var results = lines.Select(l => int.Parse(l) - 1).ToList();
            return results;
        }
    }
}
