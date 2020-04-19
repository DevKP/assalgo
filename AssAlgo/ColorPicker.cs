﻿using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class ColorPicker : Interactive
    {
        public override bool Visible { get; set; }
        public override bool Initialized { get; set; }
        public override int Z { get; set; } = 1;

        public event EventHandler<EventArgs> OnColorChanged;

        private RectangleShape _background;
        private RectangleShape _colorRect;
        private Slider _hueSlider;
        private Slider _saturationSlider;
        private Slider _brightnessSlider;

        public Color RGBColor
        {
            get => _colorRect.FillColor;
        }

        public ColorPicker(TomasEngine o) : base(o)
        {

        }
        public override void Init(TomasEngine o)
        {
            _background = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _colorRect = new RectangleShape(new Vector2f(Size.X, Size.Y * 0.25f));

            _hueSlider = o.CreateEntity<Slider>();
            _hueSlider.Z = 2;
            _saturationSlider = o.CreateEntity<Slider>();
            _saturationSlider.Z = 2;
            _brightnessSlider = o.CreateEntity<Slider>();
            _brightnessSlider.Z = 2;

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

        public override void Clicked(Vector2f localCoords)
        {
            
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(_background, states);
            target.Draw(_colorRect, states);
        }


        public override void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }

        public override void Resized()
        {
            _background.Size = Size;
            _colorRect.Size = new Vector2f(Size.X, Size.Y * 0.25f);

            _hueSlider.Position = new Vector2f(0, Size.Y * 0.25f * 1) + Position;
            _hueSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
            _saturationSlider.Position = new Vector2f(0, Size.Y * 0.25f * 2) + Position;
            _saturationSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
            _brightnessSlider.Position = new Vector2f(0, Size.Y * 0.25f * 3) + Position;
            _brightnessSlider.Size = new Vector2f(Size.X, Size.Y * 0.25f);
        }
    }
}
