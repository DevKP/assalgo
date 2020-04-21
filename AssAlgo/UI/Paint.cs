using AssAlgo.Engine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace AssAlgo
{
    class Paint : UIEntity
    {
        private bool _visible;
        public override bool Visible 
        {
            get => _visible;
            set
            {
                _visible = value;
                if (_visible)
                {
                    Position = Position + new Vector2f(0, Size.Y);
                    inAnim.Reset();
                }
            }
        }
        public override bool Initialized { get; set; }

        private Animator inAnim;

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

        public Paint(TomasEngine o, IEntity p) : base(o,p)
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

            o.MouseWheelScrolled += (_, m) => 
            {
               
                if(m.Delta > 0)
                    m.Delta = MathF.Max(_cursor.Radius * m.Delta * 0.1f, 1f);
                else
                    m.Delta = MathF.Min(_cursor.Radius * m.Delta * 0.1f, -1f);
                _brash.Radius = _cursor.Radius = _cursor.Radius + m.Delta;
                _cursor.Position = _cursor.Position - new Vector2f(m.Delta, m.Delta);
            };

            inAnim = new Animator((int)Time.FromSeconds(0.5f).AsMicroseconds());
            Initialized = true;
        }

        public override void OnEntityMouseDown(TomasEngine engine, MouseButtonEventArgs a) 
        { _lastPos = _brash.Position; }
        public override void OnEntityMouseUp(TomasEngine engine, MouseButtonEventArgs a)
        { _lastPos = new Vector2f(-1, -1); }
        public override void OnEntityMouseMoved(TomasEngine engine, MouseMoveEventArgs a)
        {
            var mCoords = _renderTexture.MapPixelToCoords(new Vector2i(a.X,a.Y));
            var LocalMouse = mCoords - GlobalPosition;
            _brash.Position = new Vector2f(LocalMouse.X - _cursor.Radius, LocalMouse.Y - _cursor.Radius);
            _cursor.Position = new Vector2f(LocalMouse.X - _cursor.Radius, LocalMouse.Y - _cursor.Radius);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                states.Transform *= Transform;
                if (mouseState == MouseButtonState.Pressed)
                {
                    _renderTexture.Draw(_brash);

                    if (_lastPos != new Vector2f(-1, -1))
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

                if (Hover && mouseState == MouseButtonState.Released)
                    target.Draw(_cursor, states);
            }
        }


        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            if(!Hover && this.mouseState == MouseButtonState.Pressed)
            {
                _lastPos = engine.ActiveWindow.MapPixelToCoords(engine.MousePosition) 
                    - GlobalPosition - new Vector2f(_cursor.Radius, _cursor.Radius);
            }
            if(Visible && !inAnim.Completed)
            {
                this.Position += new Vector2f(0, inAnim.Step((int)time.TicksDelta) * -Size.Y);
            }
            // _line.Append(new Vertex(new Vector2f(LocalMouse.X,LocalMouse.Y),_brash.FillColor));
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
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
