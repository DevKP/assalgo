using System;
using AssAlgo.Engine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AssAlgo.UI
{
    class Button : UIEntity
    {
        public override bool Visible { get; set; } = true;
        public override bool Initialized { get; set; }

        private Animator _animator;

        public event EventHandler<EventArgs> OnClicked;

        public Color BackgroundColor
        {
            get => _rect.FillColor;
            set => _rect.FillColor = value;
        }
        public string Text
        {
            get => _text.DisplayedString;
            set => _text.DisplayedString = value;
        }
        public uint TextSize
        {
            get => _text.CharacterSize;
            set => _text.CharacterSize = value;
            //Size = new Vector2f(_text.GetLocalBounds().Width, _text.GetLocalBounds().Height);
        }

        public Font TextFont
        {
            get => _text.Font;
            set => _text.Font = value;
        }

        private RectangleShape _rect;
        private Text _text;

        private VertexArray _verts;

        public Button(TomasEngine o, UIEntity p) : base(o, p)
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

            _rect = new RectangleShape(new Vector2f(Size.X, Size.Y))
            {
                FillColor = new Color(40, 40, 40),
                OutlineColor = new Color(60, 60, 60),
                OutlineThickness = 2
            };


            _text = new Text("Хоба", o.opensense, 20) {FillColor = Color.White};
            _text.Position = new Vector2f((Size.X - _text.GetGlobalBounds().Width) / 2,
                (Size.Y - _text.GetGlobalBounds().Height) / 2);

            _animator = new Animator((int)Time.FromSeconds(0.2f).AsMicroseconds());

            Initialized = true;
        }

        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            _rect.FillColor = Hover ? new Color(60, 60, 60) : new Color(45, 45, 45);

            if (MouseState == MouseButtonState.Pressed)
                _rect.FillColor = new Color(45, 45, 45);

            if(!_animator.Completed)
                this.Position += new Vector2f(_animator.Step((int)time.TicksDelta) * 400, 0);
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }


        public override void OnEntityMouseDown(TomasEngine engine, MouseButtonEventArgs a)
        {
            OnClicked?.Invoke(this, new EventArgs());
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
