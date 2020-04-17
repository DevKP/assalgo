namespace AssAlgo
{
    public class TomasTime
    {
        public float TimeScale { get; set; }
        public long EstimatedTicks { get; set; }
        public long TicksDelta { get; set; }

        public TomasTime(long delta, long ticks, float timeScale)
        {
            TimeScale = timeScale;
            EstimatedTicks = ticks;
            TicksDelta = delta;
        }
    }
}