using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public partial class Program
    {
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

           // points.Sort();
            var tt = points.SelectMany(p => points.Where(p1 => p.Item1 == p1.Item1 && p.Item2 == p1.Item2).Skip(1)).ToList();
            if (tt.Count > 0)
            {
                throw new Exception("Duplicate");
            }
            for (int i = 0; i < points.Count; i++)
            {
                res[i] = new double[points.Count];
                for (int j = 0; j < points.Count; j++)
                {
                    res[i][j] = Math.Sqrt(Math.Pow(points[i].Item1 - points[j].Item1, 2) + Math.Pow(points[i].Item2 - points[j].Item2, 2));
                    res[i][j] = (int)(res[i][j] + 0.5);
                    if (res[i][j] == 0 && i != j)
                        throw new Exception("Distance is 0");
                }
            }
            return res;
        }
        static double[][] ParseDistances1(string file)
        {
            var allLines = File.ReadAllLines(file);
            var lines = allLines.SkipWhile(s => s != "EDGE_WEIGHT_SECTION").Skip(1).TakeWhile(s => s != "EOF" && s != "DISPLAY_DATA_SECTION").ToList();

            double[][] res = new double[lines.Count][];
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
        static double[][] ParseDistances2(string file)
        {
            var allLines = File.ReadAllLines(file);
            var lines1 = allLines.SkipWhile(s => s != "EDGE_WEIGHT_SECTION").Skip(1).TakeWhile(s => s != "EOF" && s != "DISPLAY_DATA_SECTION").ToList();

            string total = ' ' + string.Join(" ", lines1).Trim();
            var lines = total.Split(new string[] { " 0" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.Insert(0, "0");

            double[][] res = new double[lines.Count][];
            for (int i = 0; i < lines.Count; i++)
            {
                var split = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                res[i] = new double[lines.Count];
                for (int j = 0; j < split.Length; j++)
                {
                    res[i][j] = double.Parse(split[j]);
                    res[j][i] = double.Parse(split[j]);
                }
            }

            return res;
        }

        static List<int> ParseSolve(string file)
        {
            try
            {
                var allLines = File.ReadAllLines(file);
                var lines = allLines.SkipWhile(s => s != "TOUR_SECTION").Skip(1).TakeWhile(s => s != "-1").ToList();
                lines.Add(lines[0]);
                var results = lines.Select(l => int.Parse(l) - 1).ToList();
                return results;
            }
            catch
            {
                return new List<int>();
            }
        }
        static List<int> ParseSolve1(string file)
        {
            try
            {
                var allLines = File.ReadAllLines(file);
                var lines = allLines.SkipWhile(s => s != "TOUR_SECTION").Skip(1).TakeWhile(s => s != "-1").ToList();

                List<int> res = new List<int>();
                foreach (var line in lines)
                {
                    var split = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    res.AddRange(split.Select(s => int.Parse(s) - 1));
                }

                res.Insert(0, res.Last());
                return res;
            }
            catch
            {
                return new List<int>();
            }
        }
    }
}
