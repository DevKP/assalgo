using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class Paint : Interactive
    {
        public override bool Visible { get; set; }
        public override bool Initialized { get; set; }
        public override int Z { get; set; } = 0;

        private RenderTexture _renderTexture;
        private RectangleShape rectangleShape;
        private CircleShape _brash;
        private CircleShape _cursor;

        private Vector2f _lastPos;

        private VertexArray _line;

        public Color BrushColor
        {
            set
            {
                _brash.FillColor = value;
            }
        }

        public Paint(TomasEngine o) : base(o)
        {

        }

        public override void Init(TomasEngine o)
        {
            _renderTexture = new RenderTexture(800, 600);

            rectangleShape = new RectangleShape(new Vector2f(500, 500));
            rectangleShape.Texture = _renderTexture.Texture;

            _brash = new CircleShape(10);
            _brash.FillColor = Color.Cyan;

            _cursor = new CircleShape(10);
            _cursor.FillColor = Color.Transparent;
            _cursor.OutlineColor = Color.Black;
            _cursor.OutlineThickness = 1;

            _line = new VertexArray(PrimitiveType.LineStrip);

            o.MouseWheelScrolled += (_, m) => _brash.Radius = _cursor.Radius = _cursor.Radius + m.Delta;
            OnMousePressed += (s, a) => _lastPos = _brash.Position;
            OnMouseReleased += (s, a) => _lastPos = new Vector2f(-1,-1);

            Initialized = true;
        }


       
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                states.Transform *= Transform;
                if (Pressed)
                {
                    _renderTexture.Draw(_brash);

                    if(_lastPos != new Vector2f(-1,-1))
                    {
                        var delta = _brash.Position - _lastPos;
                        var tmp = _brash.Position;
                        for (int i = 0; i < 20; i++)
                        {
                            _brash.Position = _lastPos + delta * (i / 20f);
                            _renderTexture.Draw(_brash);
                        }

                        _lastPos = tmp;
                    }
                    
                    _renderTexture.Display();
                }

                target.Draw(rectangleShape, states);
                //target.Draw(_line, states);

                if (Hover && !Pressed)
                    target.Draw(_cursor, states);
            }
        }

        
        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            var mCoords = _renderTexture.MapPixelToCoords(engine.MousePosition);
            var LocalMouse = mCoords - Position;
            _brash.Position = new Vector2f(LocalMouse.X - _cursor.Radius, LocalMouse.Y - _cursor.Radius);
            _cursor.Position = new Vector2f(LocalMouse.X - _cursor.Radius, LocalMouse.Y - _cursor.Radius);

            //if(Pressed)
               // _line.Append(new Vertex(new Vector2f(LocalMouse.X,LocalMouse.Y),_brash.FillColor));
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }
        public override void Clicked(Vector2f localCoords)
        {

        }

        public override void Resized()
        {
            _renderTexture = new RenderTexture((uint)Size.X, (uint)Size.Y);
            rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            rectangleShape.Texture = _renderTexture.Texture;

            _renderTexture.Clear(Color.White);
        }
    }
}
