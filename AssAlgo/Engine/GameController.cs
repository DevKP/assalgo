using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    class GameController : IEntity
    {
        public IEntity Parent { get; set; }

        public bool Initialized { get; set; }

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

        private Paint paint;
        private Button pantButton;
        private ColorPicker colorPicker;

        public void Draw(RenderTarget target, RenderStates states)
        {

        }

        public void Init(TomasEngine world)
        {


            _fuckingSlider = world.CreateEntity<Slider, UIEntity>(parent: null);
            _fuckingSlider.Position = new Vector2f(100, 100);
            _fuckingSlider.Size = new Vector2f(200, 30);

            Slider = world.CreateEntity<Slider, UIEntity>(parent: null);
            Slider.Position = new Vector2f(150, 150);
            Slider.Size = new Vector2f(200, 30);

            _fuckingText = world.CreateEntity<TomasText, UIEntity>(parent: null);
            _fuckingText.Position = new Vector2f(310, 100);
            _fuckingText.CharacterSize = 25;

            _checkerBox = world.CreateEntity<CheckerBox, UIEntity>(parent: null);
            _checkerBox.Position = new Vector2f(500, 200);
            _checkerBox.Size = new Vector2f(15, 15);

            _fuckingText1 = world.CreateEntity<TomasText, UIEntity>(parent: null);
            _fuckingText1.Position = new Vector2f(520, 200);
            _fuckingText1.CharacterSize = 13;
            _fuckingText1.Text = "Smoke";

            _checkerBox.OnStateChanged += (c, _) => _fuckingText1.Visible = (c as CheckerBox).Checked;

            _checkerBox1 = world.CreateEntity<CheckerBox, UIEntity>(parent: null);
            _checkerBox1.Position = new Vector2f(500, 220);
            _checkerBox1.Size = new Vector2f(15, 15);

            _fuckingText2 = world.CreateEntity<TomasText, UIEntity>(parent: null);
            _fuckingText2.Position = new Vector2f(520, 220);
            _fuckingText2.CharacterSize = 13;
            _fuckingText2.Text = "Weed";

            _checkerBox1.OnStateChanged += (c, _) => _fuckingText2.Visible = (c as CheckerBox).Checked;
            _checkerBox2 = world.CreateEntity<CheckerBox, UIEntity>(parent: null);
            _checkerBox2.Position = new Vector2f(500, 240);
            _checkerBox2.Size = new Vector2f(15, 15);

            _fuckingText3 = world.CreateEntity<TomasText, UIEntity>(parent: null);
            _fuckingText3.Position = new Vector2f(520, 240);
            _fuckingText3.CharacterSize = 13;
            _fuckingText3.Text = "Everyday";

            _checkerBox2.OnStateChanged += (c, _) => _fuckingText3.Visible = (c as CheckerBox).Checked;
            _checkerBox3 = world.CreateEntity<CheckerBox, UIEntity>(parent: null);
            _checkerBox3.Position = new Vector2f(500, 260);
            _checkerBox3.Size = new Vector2f(15, 15);

            _fuckingText4 = world.CreateEntity<TomasText, UIEntity>(parent: null);
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


            paint = world.CreateEntity<Paint, UIEntity>(parent: null);
            paint.Size = new Vector2f(100, 100);
            paint.Z = 2;

            pantButton = world.CreateEntity<Button, UIEntity>(parent: paint);
            pantButton.Size = new Vector2f(100, 30);
            pantButton.Position = new Vector2f(10, 550);
            pantButton.Text = "Paint!";
            pantButton.TextSize = 16;
            pantButton.TextFont = world.opensense_reg;


            colorPicker = world.CreateEntity<ColorPicker, UIEntity>(parent: paint);
            colorPicker.Position = new Vector2f(600, 450);
            colorPicker.Size = new Vector2f(150, 100);
            colorPicker.OnColorChanged += (c, _) => paint.BrushColor = (c as ColorPicker).RGBColor;

            pantButton.OnClicked += (_, x) => colorPicker.Visible = paint.Visible = !paint.Visible;

            _splash = world.CreateEntity<SplashScreen, UIEntity>(parent: null);
            _splash.Z = 420;
            _clock = new Clock();

            world.OnResized += Tooo_OnResized;

            Initialized = true;
        }

        private void Tooo_OnResized(object sender, SFML.Window.SizeEventArgs e)
        {
            pantButton.Position = new Vector2f(10, e.Height - pantButton.Size.Y - 10);
            colorPicker.Position = new Vector2f(e.Width - colorPicker.Size.X - 10, e.Height - colorPicker.Size.Y - 10);
            colorPicker.Size = colorPicker.Size;
            paint.Size = new Vector2f(e.Width, e.Height);
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
