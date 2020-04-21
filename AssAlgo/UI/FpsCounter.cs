using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    public class FpsCounter : Transformable, IEntity
    {
        public IEntity Parent { get; set; }

        public bool Initialized { get; set; }
        public bool Visible { get; set; }
        public int Z { get; set; } = 100;

        private Text _fpsText;
        private Text _deltaDrawText;
        private Text _deltaUpdateText;
        private Text _objText;

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_fpsText, states);
            target.Draw(_deltaDrawText, states);
            target.Draw(_deltaUpdateText, states);
            target.Draw(_objText, states);
        }

        public void Init(TomasEngine engine)
        {
            _fpsText = new Text("0FPS", engine.arial)
            {
                CharacterSize = 24,
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            _deltaDrawText = new Text("0ms", engine.arial)
            {
                CharacterSize = 24,
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            _deltaUpdateText = new Text("0ms", engine.arial)
            {
                CharacterSize = 24,
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };
            _objText = new Text("0 Enities", engine.arial)
            {
                CharacterSize = 24,
                FillColor = Color.White,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };


            _deltaDrawText.Position = new Vector2f(0, _fpsText.GetGlobalBounds().Height + 5);
            _deltaUpdateText.Position = new Vector2f(0, _fpsText.GetGlobalBounds().Height + 
                _deltaUpdateText.GetGlobalBounds().Height + 10);
            _objText.Position = new Vector2f(0, _fpsText.GetGlobalBounds().Height * 3 + 15);
            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            _fpsText.DisplayedString = (1000000 / time.TicksDelta) + "FPS";
            _deltaDrawText.DisplayedString = (time.LastDrawTime / 1000f) + "ms";
            _deltaUpdateText.DisplayedString = (time.LastUpdateTime / 1000f) + "ms";
            _objText.DisplayedString = engine.EntityNumber + " Enities";
            Position = new Vector2f(engine.WindowsSize.X - _objText.GetLocalBounds().Width, 0);
        }

        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }
    }
}