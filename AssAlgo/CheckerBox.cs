using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class CheckerBox : Interactive
    {

        public override bool Visible { get; set; }
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

        public CheckerBox(TomasEngine e) : base(e)
        {

        }

        public override void Clicked(Vector2f localCoords)
        {

        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_background, states);
        }

        public override void Init(TomasEngine o)
        {
            _background = new RectangleShape();
            
            _background.FillColor = new Color(45, 45, 45);
            _background.OutlineThickness = 1;
            _background.OutlineColor = new Color(60, 60, 60);

            Size = new Vector2f(50, 50);

            OnMousePressed += (_, x) =>
            {
                var mCoords = o.ActiveWindow.MapPixelToCoords(o.MousePosition);
                var sliderRect = _background.GetGlobalBounds();
                sliderRect.Left += Position.X;
                sliderRect.Top += Position.Y;
                if (PointInRectangle(sliderRect, mCoords))
                {
                    _checked = !_checked;
                    OnStateChanged?.Invoke(this, new EventArgs());
                }
            };

            //anotherSlider = o.CreateEntity<Slider>();
            //anotherSlider.Position = new Vector2f(200, 200);

            Initialized = true;
        }

        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {

            var sliderLocal = engine.ActiveWindow.MapPixelToCoords(engine.MousePosition) - Position;
            var sliderRect = _background.GetGlobalBounds();
            if (PointInRectangle(sliderRect, sliderLocal))
                _background.FillColor = new Color(60, 60, 60);
            else
                _background.FillColor = new Color(45, 45, 45);

            if(_checked)
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
