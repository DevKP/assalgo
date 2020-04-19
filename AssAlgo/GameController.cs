using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AssAlgo
{
    class GameController : IEntity
    {
        public bool Initialized  { get; set;}

        public bool Visible { get; set; }
        public int Z { get; set; } = 0;

        private SplashScreen _splash;
        private Clock _clock;

        Slider _fuckingSlider;
        Slider Slider;
        TomasText _fuckingText;
        TomasText _fuckingText1;
        TomasText _fuckingText2;
        TomasText _fuckingText3;
        TomasText _fuckingText4;
        CheckerBox _checkerBox;
        CheckerBox _checkerBox1;
        CheckerBox _checkerBox2;
        CheckerBox _checkerBox3;

        public void Draw(RenderTarget target, RenderStates states)
        {
            
        }

        public void Init(TomasEngine tooo)
        {
            

            _fuckingSlider = tooo.CreateEntity<Slider>();
            _fuckingSlider.Position = new Vector2f(100,100);
            _fuckingSlider.Size = new Vector2f(200, 30);

            Slider = tooo.CreateEntity<Slider>();
            Slider.Position = new Vector2f(150, 150);
            Slider.Size = new Vector2f(200, 30);

            _fuckingText = tooo.CreateEntity<TomasText>();
            _fuckingText.Position = new Vector2f(310, 100);
            _fuckingText.CharacterSize = 25;

            _checkerBox = tooo.CreateEntity<CheckerBox>();
            _checkerBox.Position = new Vector2f(500, 200);
            _checkerBox.Size = new Vector2f(15, 15);

            _fuckingText1 = tooo.CreateEntity<TomasText>();
            _fuckingText1.Position = new Vector2f(520, 200);
            _fuckingText1.CharacterSize = 13;
            _fuckingText1.Text = "Smoke";

            _checkerBox.OnStateChanged += (c, _) => _fuckingText1.Visible = (c as CheckerBox).Checked;

            _checkerBox1 = tooo.CreateEntity<CheckerBox>();
            _checkerBox1.Position = new Vector2f(500, 220);
            _checkerBox1.Size = new Vector2f(15, 15);

            _fuckingText2 = tooo.CreateEntity<TomasText>();
            _fuckingText2.Position = new Vector2f(520, 220);
            _fuckingText2.CharacterSize = 13;
            _fuckingText2.Text = "Weed";

            _checkerBox1.OnStateChanged += (c, _) => _fuckingText2.Visible = (c as CheckerBox).Checked;
            _checkerBox2 = tooo.CreateEntity<CheckerBox>();
            _checkerBox2.Position = new Vector2f(500, 240);
            _checkerBox2.Size = new Vector2f(15, 15);

            _fuckingText3 = tooo.CreateEntity<TomasText>();
            _fuckingText3.Position = new Vector2f(520, 240);
            _fuckingText3.CharacterSize = 13;
            _fuckingText3.Text = "Everyday";

            _checkerBox2.OnStateChanged += (c, _) => _fuckingText3.Visible = (c as CheckerBox).Checked;
            _checkerBox3 = tooo.CreateEntity<CheckerBox>();
            _checkerBox3.Position = new Vector2f(500, 260);
            _checkerBox3.Size = new Vector2f(15, 15);

            _fuckingText4 = tooo.CreateEntity<TomasText>();
            _fuckingText4.Position = new Vector2f(520, 260);
            _fuckingText4.CharacterSize = 13;
            _fuckingText4.Text = "Bro :D";

            _checkerBox3.OnStateChanged +=
                (c, _) =>
                    _fuckingText4.Visible = (c as CheckerBox).Checked;

            _fuckingSlider.OnValueChanged +=
                (s, _) =>
                {
                    Slider.Value = (s as Slider).Value;
                    _fuckingText.Text = Slider.Value.ToString();
                    if (Slider.Value == 1000)
                        _fuckingText.Text = "Дима пидр!";
                };

            Slider.OnValueChanged +=
                (s, _) =>
                {
                    _fuckingSlider.Value = (s as Slider).Value;
                    _fuckingText.Text = Slider.Value.ToString();
                };


            var paint = tooo.CreateEntity<Paint>();
            paint.Size = new Vector2f(400, 400);

            var but = tooo.CreateEntity<Button>();
            but.Size = new Vector2f(100, 30);
            but.Position = new Vector2f(10, 410);
            but.Text = "Paint!";
            but.TextSize = 16;
            but.TextFont = tooo.opensense_reg;

            but.OnClicked += (_, x) => paint.Visible = !paint.Visible;


            var cp = tooo.CreateEntity<ColorPicker>();
            cp.Position = new Vector2f(150, 410);
            cp.Size = new Vector2f(150, 100);
            cp.OnColorChanged += (c, _) => paint.BrushColor = (c as ColorPicker).RGBColor;


            _splash = tooo.CreateEntity<SplashScreen>();
            _splash.Z = 10;
            _clock = new Clock();

            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            if (_clock.ElapsedTime.AsSeconds() > 3)
                _splash.Visible = false;

            //var view = engine.ActiveWindow.GetView();
            //var Viewport = view.Viewport;
            //view.Viewport = new FloatRect(Viewport.Left + 0.001f, Viewport.Top + 0.001f, Viewport.Width, Viewport.Height);
            //engine.ActiveWindow.SetView(view);
           // engine.ActiveWindow.SetView(new View(new FloatRect(view.Left + 0.01f, view.Top + 0.01f, view.Width, view.Height)));
        }

        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }
    }
}
