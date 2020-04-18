using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    public class TestEntity : Transformable, IEntity
    {
        public bool Initialized { get; set; }
        public bool Visible { get; set; }

        private Text _testText;
        private Clock _clock;

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_testText, states);
        }

        public void Init(TomasEngine engine)
        {
            _testText = new Text("Test", new Font("Arial.ttf"))
            {
                CharacterSize = 24,
                FillColor = Color.Black
            };
            _clock = new Clock();
            

            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            _testText.DisplayedString = _clock.ElapsedTime.AsMilliseconds().ToString()+"ms";
        }

        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }
    }
}