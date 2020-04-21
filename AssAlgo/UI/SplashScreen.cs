using SFML.Graphics;
using SFML.System;
using System;

namespace AssAlgo
{
    class SplashScreen : UIEntity
    {
        public override bool Visible { get; set; } = true;
        public override bool Initialized { get; set; }

        private Texture tex;
        private RectangleShape _rectangleShape;
        private RectangleShape _logo;
        private Clock clock;

        public SplashScreen(TomasEngine o, IEntity p) : base(o, p) { }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                target.Draw(_rectangleShape, states);
                //states.Transform = _logo.Transform;
                target.Draw(_logo, states);
            }
        }

        public override void Init(TomasEngine o)
        {
            _rectangleShape = new RectangleShape(new Vector2f(o.WindowsSize.X, o.WindowsSize.Y))
            {
                FillColor = new Color(45, 45, 45)
            };
            tex = new Texture("splash.png");

            _logo = new RectangleShape(new Vector2f(tex.Size.X, tex.Size.Y))
            {
                Texture = tex
            };
            _logo.Position = new Vector2f((_rectangleShape.Size.X - _logo.Size.X) / 2,
                (_rectangleShape.Size.Y - _logo.Size.Y) / 2);

            Size = _rectangleShape.Size;

            clock = new Clock();

            Initialized = true;
        }

        public override void UIUpdate(TomasEngine engine, TomasTime time)
        {
            float animationDuration = 0.8f;
            var a = clock.ElapsedTime.AsSeconds() / animationDuration;

            var mass = 1;
            var stiffness = MathF.Pow(2 * MathF.PI / 60, 2) * mass;
            var damping = 4 * MathF.PI * 0.1f * mass / 60;


            if (a < 1)
            {
                var animScale = -MathF.Cos(a * MathF.PI) / 2f + 0.5f;


                //_logo.Position = new Vector2f((MathF.Cos(a * MathF.PI ) + 2) / 2 * pathVector.X, 0);
                _logo.Size = new Vector2f(animScale * tex.Size.X * 4, animScale * tex.Size.X * 4);
                _logo.Rotation += 50;

                _logo.Position = new Vector2f((_rectangleShape.Size.X - _logo.Size.X) / 2,
                    (_rectangleShape.Size.Y - _logo.Size.Y) / 2);

                //_rectangleShape.FillColor = new Color(45, 45, 45, (byte) (255 * animScale));
            }
            if (a > 3 && a < 4)
            {
                var b = a - 3;

                var animScale = MathF.Cos(b * MathF.PI) / 2f + 0.5f;
                _rectangleShape.FillColor = new Color(45, 45, 45, (byte)(255 * animScale));
                _logo.FillColor = new Color(255, 255, 255, (byte)(255 * animScale));
            }
        }

        public override void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }

        public override void Resized()
        {

        }
    }
}