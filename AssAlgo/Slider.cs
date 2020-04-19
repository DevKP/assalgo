using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    enum SliderState
    {
        Resting,
        Hovering,
        Moving
    }

    class Slider : Interactive
    {
        public override bool Visible { get; set; }
        public override bool Initialized { get; set; }

        public event EventHandler<EventArgs> OnValueChanged;

        private RectangleShape _background;
        private RectangleShape _sliderPath;
        private RectangleShape _slider;

        private float _value = 500;
        private SliderState _state;

        public float Value
        {
            get => _value;
            set
            {
                _value = value;
                
            }
        }

        public Slider(TomasEngine e) : base(e)
        {

        }

        public override void Clicked(Vector2f localCoords)
        {
            
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_background, states);
            target.Draw(_sliderPath, states);
            target.Draw(_slider, states);
        }

        public override void Init(TomasEngine o)
        {
            _state = SliderState.Resting;

            _background = new RectangleShape();
            _slider = new RectangleShape();
            _sliderPath = new RectangleShape();

            _background.FillColor = new Color(60, 60, 60);
            _slider.FillColor = new Color(85, 85, 85);
            _sliderPath.FillColor = new Color(30, 30, 30);

            Size = new Vector2f(500, 60);

            OnMousePressed += (_, x) => 
            {
                var mCoords = o.ActiveWindow.MapPixelToCoords(o.MousePosition);
                var sliderRect = _sliderPath.GetGlobalBounds();
                sliderRect.Left += Position.X;
                sliderRect.Top += Position.Y;
                if (PointInRectangle(sliderRect, mCoords))
                    _state = SliderState.Moving;
            };

            OnMouseReleased += (_, x) =>
            {
                _state = SliderState.Resting;
            };

            //anotherSlider = o.CreateEntity<Slider>();
            //anotherSlider.Position = new Vector2f(200, 200);

            Initialized = true;
        }

        private float RangeLimitF(float bottom, float value, float top)
        {
            if (value < bottom)
                return bottom;
            else if (value > top)
                return top;
            return value;
        }
        private float MapToRangeF(float from, float to, float value)
        {
            return to / from * value;
        }

        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            var sliderCenterPos = _slider.Size / 2;
            _slider.Position = new Vector2f(MapToRangeF(1000, _sliderPath.Size.X - _slider.Size.X, _value) +
                _sliderPath.Position.X, _slider.Position.Y);

            if (_state != SliderState.Moving)
            {
                var sliderLocal = engine.ActiveWindow.MapPixelToCoords(engine.MousePosition) - Position;
                var sliderRect = _slider.GetGlobalBounds();
                if (PointInRectangle(sliderRect, sliderLocal))
                    _state = SliderState.Hovering;
                else
                    _state = SliderState.Resting;
            }

            switch (_state)
            {
                case SliderState.Resting:
                    _slider.FillColor = new Color(85, 85, 85);
                    break;
                case SliderState.Moving:
                    var mouseLocal = engine.ActiveWindow.MapPixelToCoords(engine.MousePosition) - Position - _sliderPath.Position - _slider.Size / 2;
                    _value = RangeLimitF(0, MapToRangeF(_sliderPath.Size.X - _slider.Size.X, 1000, mouseLocal.X), 1000);
                    OnValueChanged?.Invoke(this, new EventArgs());
                    break;
                case SliderState.Hovering:
                    _slider.FillColor = new Color(100, 100, 100);
                    break;
            }     
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }

        public override void Resized()
        {
            _background.Size = Size;

            _sliderPath.Size = new Vector2f(Size.X * 0.8f, Size.Y * 0.5f);
            var sizeDiff = Size - _sliderPath.Size;
            _sliderPath.Position = new Vector2f(sizeDiff.X / 2, sizeDiff.Y / 2);

            var sliderSize = MathF.Min(_sliderPath.Size.X, _sliderPath.Size.Y);
            _slider.Position = _sliderPath.Position;
            _slider.Size = new Vector2f(sliderSize, sliderSize);
        }
    }
}
