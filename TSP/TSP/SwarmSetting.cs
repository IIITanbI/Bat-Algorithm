namespace TSP
{
    public class SwarmSetting
    {
        public double[][] CostMatrix { get; set; }
        public int SwarmSize { get; set; }
        public double Alpha { get; set; } = 0.98;
        public double Gamma { get; set; } = 0.98;

        public double InitialR { get; set; }
    }
}
