using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace AssAlgo
{
    class CheckerBox : UIEntity
    {

        public override bool Visible { get; set; } = true;
        public override bool Initialized { get; set; }

        public event EventHandler<EventArgs> OnStateChanged;

        private RectangleShape _background;

        private bool _checked = false;

        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;

            }
        }

        public CheckerBox(TomasEngine e, IEntity p) : base(e, p)
        {

        }
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                states.Transform *= Transform;
                target.Draw(_background, states);
            }
        }

        public override void Init(TomasEngine o)
        {
            _background = new RectangleShape();

            _background.FillColor = new Color(45, 45, 45);
            _background.OutlineThickness = 1;
            _background.OutlineColor = new Color(60, 60, 60);

            Size = new Vector2f(50, 50);

            Initialized = true;
        }

        public override void OnEntityMouseDown(TomasEngine engine, MouseButtonEventArgs a)
        {
            _checked = !_checked;
            OnStateChanged?.Invoke(this, new EventArgs());
        }

        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            if (Hover)
                _background.FillColor = new Color(60, 60, 60);
            else
                _background.FillColor = new Color(45, 45, 45);

            if (_checked)
                _background.FillColor = new Color(80, 80, 80);
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }

        public override void Resized()
        {
            _background.Size = Size;
        }
    }
}
