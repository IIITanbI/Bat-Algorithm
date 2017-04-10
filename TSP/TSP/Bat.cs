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
        public double V { get; set; }
        /// <summary>
        /// Loudness
        /// </summary>
        public double A { get; set; }


        public Path Path { get; set; } = new Path();
    }
}
