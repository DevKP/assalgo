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

        public Color BackgroundColor 
        {
            get => _rect.FillColor;
            set => _rect.FillColor = value;
        }

        private RectangleShape _rect;
        private Text _text;

        private VertexArray _verts;

        public Button(TomasEngine o) : base(o)
        {
        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_rect, states);
            target.Draw(_text, states);
        }

        public override void Init(TomasEngine o)
        {
            _verts = new VertexArray(PrimitiveType.Quads, 4);
            
            _rect = new RectangleShape(new SFML.System.Vector2f(Size.X, Size.Y));
            _rect.FillColor = new Color(40,40,40);
            _rect.OutlineColor = new Color(60, 60, 60);
            _rect.OutlineThickness = 2;
            

            _text = new Text("Хоба", o.opensense, 20);
            _text.FillColor = Color.White;
            _text.Position = new Vector2f((Size.X - _text.GetGlobalBounds().Width) / 2,
                (Size.Y - _text.GetGlobalBounds().Height) / 2);

            CanDrag = true;
            Initialized = true;
        }

        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            if (Hover)
                _rect.FillColor = new Color(60, 60, 60);
            else
                _rect.FillColor = new Color(45, 45, 45);

            if (Pressed)
                _rect.FillColor = new Color(45, 45, 45);
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
            _rect.Size = Size;
            _text.Position = new Vector2f((Size.X - _text.GetLocalBounds().Width) / 2,
                (Size.Y - _text.GetLocalBounds().Height) / 2);
            _verts.Clear();
            _verts.Append(new Vertex(new Vector2f(), Color.Blue));
            _verts.Append(new Vertex(new Vector2f(Size.X, 0), Color.Green));
            _verts.Append(new Vertex(new Vector2f(Size.X, Size.Y), Color.Green));
            _verts.Append(new Vertex(new Vector2f(0, Size.Y), Color.Blue));
        }
    }
}
