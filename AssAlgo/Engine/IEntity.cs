using SFML.Graphics;

namespace AssAlgo
{
    public interface IEntity : Drawable
    {
        public IEntity Parent { get; set; }

        public int Z { get; set; }
        public bool Initialized { get; }
        public bool Visible { get; set; }
        void Init(TomasEngine o);
        void LogicUpdate(TomasEngine engine, TomasTime time);
        void LogicUpdateAsync(TomasEngine engine, TomasTime time);
    }

    public class UpdateEventArgs
    {
        public TomasTime EngineTime { get; }
        public UpdateEventArgs(TomasTime time)
            => EngineTime = time;
    }
}
