using SFML.Graphics;
using SFML.System;
using System;

namespace AssAlgo
{
    class ColorPicker : UIEntity
    {
        public override bool Visible { get; set; }
        public override bool Initialized { get; set; }

        public event EventHandler<EventArgs> OnColorChanged;

        private RectangleShape _background;
        private RectangleShape _colorRect;
        private Slider _hueSlider;
        private Slider _saturationSlider;
        private Slider _brightnessSlider;

        public Color RGBColor => _colorRect.FillColor;

        public ColorPicker(TomasEngine o, UIEntity parent) : base(o, parent)
        {

        }
        public override void Init(TomasEngine engine)
        {
            _background = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _background.OutlineThickness = 1;
            _background.OutlineColor = new Color(120, 120, 120);
            _colorRect = new RectangleShape(new Vector2f(Size.X, Size.Y * 0.25f));

            _hueSlider = engine.CreateEntity<Slider, UIEntity>(this);
            _saturationSlider = engine.CreateEntity<Slider, UIEntity>(this);
            _brightnessSlider = engine.CreateEntity<Slider, UIEntity>(this);

            _hueSlider.OnValueChanged += OnValueChanged;
            _saturationSlider.OnValueChanged += OnValueChanged;
            _brightnessSlider.OnValueChanged += OnValueChanged;

            Size = new Vector2f(150, 100);

            Initialized = true;
        }

        private void OnValueChanged(object sender, EventArgs e)
        {
            _colorRect.FillColor = _hslToRgb(_hueSlider.Value / 1000f,
                _saturationSlider.Value / 1000f,
                _brightnessSlider.Value / 1000f);
            OnColorChanged?.Invoke(this, new EventArgs());
        }

        private float _hue2rgb(float p, float q, float t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1f / 6f) return p + (q - p) * 6 * t;
            if (t < 1f / 2f) return q;
            if (t < 2f / 3f) return p + (q - p) * (2f / 3f - t) * 6;
            return p;
        }

        private Color _hslToRgb(float h, float s, float l)
        {
            float r, g, b;

            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
                var p = 2 * l - q;
                r = _hue2rgb(p, q, h + 1f / 3f);
                g = _hue2rgb(p, q, h);
                b = _hue2rgb(p, q, h - 1f / 3f);
            }

            return new Color((byte)Math.Round(r * 255),
                (byte)Math.Round(g * 255),
                (byte)Math.Round(b * 255));
        }


        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                states.Transform *= Transform;
                target.Draw(_background, states);
                target.Draw(_colorRect, states);
            }
        }


        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            _hueSlider.Visible = Visible;
            _saturationSlider.Visible = Visible;
            _brightnessSlider.Visible = Visible;
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }

        public override void Resized()
        {
            _background.Size = Size;
            _colorRect.Size = new Vector2f(Size.X, Size.Y * 0.25f);

            _hueSlider.Position = new Vector2f(0, Size.Y * 0.25f * 1);
            _hueSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
            _saturationSlider.Position = new Vector2f(0, Size.Y * 0.25f * 2);
            _saturationSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
            _brightnessSlider.Position = new Vector2f(0, Size.Y * 0.25f * 3);
            _brightnessSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
        }
    }
}
