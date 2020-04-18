using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    public class FpsCounter : Transformable, IEntity
    {
        public bool Initialized { get; set; }
        public bool Visible { get; set; }

        private Text _fpsText;

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_fpsText, states);
        }

        public void Init(TomasEngine engine)
        {
            _fpsText = new Text("0FPS", new Font("Arial.ttf"))
            {
                CharacterSize = 24,
                FillColor = Color.Black
            };

            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            _fpsText.DisplayedString = (1000000 / time.TicksDelta) + "FPS";
            Position = new Vector2f(engine.WindowsSize.X - _fpsText.GetGlobalBounds().Width, 0);
        }

        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }
    }
}