using System;
using System.Numerics;
using AssAlgo.Engine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AssAlgo.UI
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
                if (!_visible) return;
                Position += new Vector2f(0, Size.Y);
                _inAnim.Reset();
            }
        }
        public override bool Initialized { get; set; }

        private Animator _inAnim;

        private RenderTexture _renderTexture;
        private RectangleShape _rectangleShape;
        private CircleShape _brash;
        private CircleShape _cursor;

        private Vector2f _lastPos;

        private VertexArray _line;
        private Vector2f _lastClick;

        public Color BrushColor
        {
            set => _brash.FillColor = value;
        }

        public Paint(TomasEngine o, IEntity p) : base(o,p)
        {

        }

        public override void Init(TomasEngine o)
        {
            _renderTexture = new RenderTexture(800, 600);

            _rectangleShape = new RectangleShape(new Vector2f(500, 500))
            {
                Texture = _renderTexture.Texture
            };

            _brash = new CircleShape(10)
            {
                FillColor = Color.Cyan
            };

            _cursor = new CircleShape(10)
            {
                FillColor = Color.Transparent,
                OutlineColor = Color.Black,
                OutlineThickness = 1
            };

            _line = new VertexArray(PrimitiveType.LineStrip);

            o.MouseWheelScrolled += (_, m) =>
            {
                m.Delta = _cursor.Radius * m.Delta * 0.1f;
                _brash.Radius = MathF.Abs(_cursor.Radius += m.Delta);
                _cursor.Position -= new Vector2f(m.Delta, m.Delta);
            };

            _inAnim = new Animator((int)Time.FromSeconds(0.5f).AsMicroseconds());
            Initialized = true;
        }

        public override void OnEntityMouseDown(TomasEngine engine, MouseButtonEventArgs a)
        {
            _lastPos = _brash.Position;
            var tmp = _brash.Position;

            if (KeyboardState == KeyboardButtonState.Shift)
            {
                if (_lastClick != new Vector2f(-1, -1))
                {
                    var delta = _brash.Position - _lastClick;
                    float iterStep = _brash.Radius / 2;

                    var deltaMag = MathF.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                    var iterCount = deltaMag / iterStep;

                    //var pos = _lastClick + delta * 0.5f;
                    //RectangleShape _line = new RectangleShape
                    //{
                    //    Size = new Vector2f(50, 50),
                    //    Position = pos + new Vector2f(_brash.Radius * 2, 0),
                    //    FillColor = Color.Black,
                    //    Rotation = MathF.Atan2(delta.Y, delta.X) * (180 / MathF.PI)

                    //};
                    //_renderTexture.Draw(_line);
                    for (var i = 0; i < iterCount; i++)
                    {
                        _brash.Position = _lastClick + delta * (i / (iterCount));
                        _renderTexture.Draw(_brash);
                    }
                }
                _renderTexture.Display();
            }

            _lastClick = _brash.Position = tmp;
        }
        public override void OnEntityMouseUp(TomasEngine engine, MouseButtonEventArgs a)
        { _lastPos = new Vector2f(-1, -1); }
        public override void OnEntityMouseMoved(TomasEngine engine, MouseMoveEventArgs a)
        {
            var mCoords = _renderTexture.MapPixelToCoords(new Vector2i(a.X,a.Y));
            var localMouse = mCoords - GlobalPosition;
            _brash.Position = new Vector2f(localMouse.X - _cursor.Radius, localMouse.Y - _cursor.Radius);
            _cursor.Position = new Vector2f(localMouse.X - _cursor.Radius, localMouse.Y - _cursor.Radius);
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (!Visible) return;

            states.Transform *= Transform;
            if (MouseState == MouseButtonState.Pressed)
            {
                _renderTexture.Draw(_brash);

                if (_lastPos != new Vector2f(-1, -1))
                {
                    var delta = _brash.Position - _lastPos;
                    var tmp = _brash.Position;
                    float iterStep = _brash.Radius / 2;

                    var deltaMag = MathF.Sqrt(delta.X * delta.X + delta.Y * delta.Y);
                    var iterCount = deltaMag / iterStep;
                    for (var i = 2; i < iterCount; i++)
                    {
                        _brash.Position = _lastPos + delta * (i / (iterCount));
                        _renderTexture.Draw(_brash);
                    }

                    _lastPos = tmp;
                }

                _renderTexture.Display();
            }

            target.Draw(_rectangleShape, states);
            //target.Draw(_line, states);

            if (Hover && MouseState == MouseButtonState.Released)
                target.Draw(_cursor, states);
        }


        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            if(!Hover && this.MouseState == MouseButtonState.Pressed)
            {
                _lastPos = engine.ActiveWindow.MapPixelToCoords(engine.MousePosition) 
                    - GlobalPosition - new Vector2f(_cursor.Radius, _cursor.Radius);
            }
            if(Visible && !_inAnim.Completed)
            {
                this.Position += new Vector2f(0, _inAnim.Step((int)time.TicksDelta) * -Size.Y);
            }
            // _line.Append(new Vertex(new Vector2f(LocalMouse.X,LocalMouse.Y),_brash.FillColor));
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }

        public override void Resized()
        {
            _renderTexture = new RenderTexture((uint)Size.X, (uint)Size.Y);
            _rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _rectangleShape.Texture = _renderTexture.Texture;

            _renderTexture.Clear(Color.White);
        }
    }
}
