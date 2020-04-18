using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class Button : Interactive
    {
        public override bool Visible { get; set; }
        public override bool Initialized { get; set; }

        private RectangleShape _rect;
        private Text _text;

        public Button(TomasEngine o) : base(o)
        {
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Hover)
                _rect.FillColor = Color.Red;
            else
                _rect.FillColor = Color.Green;

            if (Pressed)
                _rect.FillColor = Color.Blue;

            states.Transform *= Transform;
            target.Draw(_rect, states);
            target.Draw(_text, states);
        }

        public override void Init(TomasEngine o)
        {
            _rect = new RectangleShape(new SFML.System.Vector2f(Size.X, Size.Y));
            _rect.FillColor = Color.Green;

            _text = new Text("Я ебу собак",new Font("Arial.ttf"), 13);
            _text.FillColor = Color.Black;
            _text.Position = new Vector2f((Size.X - _text.GetLocalBounds().Width) / 2,
                (Size.Y - _text.GetLocalBounds().Height) / 2);

            CanDrag = true;
            Initialized = true;
        }

        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {
           
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }

        public override void Clicked(Vector2f localCoords)
        {
            //_rect = new RectangleShape(new SFML.System.Vector2f(localCoords.X, localCoords.Y));
        }

        public override void Resized()
        {
            
        }
    }
}
