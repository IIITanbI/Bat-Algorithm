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
        public Path Path { get; set; } = new Path();
        public double Cost { get; private set; } = 0;

        public Bat(int startCity)
        {
            CurrentCity = startCity;
            Path.Cities.Add(startCity);
        }

        public void NextCity(double[][] cost)
        {
            if (CurrentCity == -1)
                throw new System.Exception($"CurrentCity = -1");

            int bestCost = -1;
            
            for (int i = 0; i < cost.GetLength(CurrentCity); i++)
            {
                int from = CurrentCity;
                int to = i;
                double c = cost[from][to];
                if (c == -1)
                    continue;

                var newPath = new Path(Path);
                newPath.Cities.Add(to);

                double tempCost1 = Cost + c;

                var modifiedP = Helper.TwoOpt(newPath);
                double tempCost2 = modifiedP.GetCost(cost) + c;

                if (tempCost1 <= tempCost2)
                {

                }

            }
        }
    }
}
