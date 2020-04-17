using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AssAlgo
{

    public class TomasEngine
    {
        private RenderWindow _window;
        private Color _clearColor;

        private List<IEntity> _entities;
        private int _targetFramerate;
        private long _targetMicrosec;
        private Vector2u _windowsSize;

        /// <summary>Event handler for the TextEntered event</summary>
        public event EventHandler<TextEventArgs> TextEntered;

        /// <summary>Event handler for the KeyPressed event</summary>
        public event EventHandler<KeyEventArgs> KeyPressed;

        /// <summary>Event handler for the KeyReleased event</summary>
        public event EventHandler<KeyEventArgs> KeyReleased;

        /// <summary>Event handler for the MouseWheelScrolled event</summary>
        public event EventHandler<MouseWheelScrollEventArgs> MouseWheelScrolled;

        /// <summary>Event handler for the MouseButtonPressed event</summary>
        public event EventHandler<MouseButtonEventArgs> MouseButtonPressed;

        /// <summary>Event handler for the MouseButtonReleased event</summary>
        public event EventHandler<MouseButtonEventArgs> MouseButtonReleased;

        /// <summary>Event handler for the MouseMoved event</summary>
        public event EventHandler<MouseMoveEventArgs> MouseMoved = delegate { };

        public Color ClearColor
        {
            get => _clearColor;
            set => _clearColor = value;
        }

        public int TargetFramerate
        {
            get => _targetFramerate;
            set
            {
                _targetFramerate = value;
                _targetMicrosec = Time.FromSeconds(1).AsMicroseconds() / _targetFramerate;
            }
        }

        public Vector2u WindowsSize => _windowsSize;

        public TomasEngine(string title, uint width, uint height, VideoMode mode)
        {
            _window = new RenderWindow(mode, title);
            _window.SetVisible(false);
            _window.Size = new Vector2u(width,height);
            _window.SetView(new View(new FloatRect(0f,0f, width, height)));
            _window.Closed += (o, _) => _window.Close();

            _clearColor = Color.White;
            TargetFramerate = 60;

            _entities = new List<IEntity>();
        }

        public IEntity GetEntity(int id)
        {
            return _entities[id];
        }

        public IEntity GetEntity<T>()
        {
            return _entities.First(entity => entity is T);
        }

        public int HandleEntity(IEntity entity)
        {
            _entities.Add(entity);
            return _entities.Count;
        }
        public void Run()
        {
            _window.MouseMoved += (o, a) => MouseMoved.Invoke(this, a);
            _window.Resized += (_, x) =>
            {
                _window.SetView(new View(new FloatRect(0f, 0f, x.Width, x.Height)));
                _windowsSize = new Vector2u(x.Width,x.Height);
            };
            _window.MouseButtonPressed += (o, a) => MouseButtonPressed?.Invoke(this, a);

            InitEntities();

            _window.SetVisible(true);

            Clock engineClock = new Clock();
            Clock deltaClock = new Clock();

            while (_window.IsOpen)
            {
                _window.DispatchEvents();

                _window.Clear(_clearColor);

                TomasTime tomasTime = new TomasTime(0, 0, 1.0f)
                {
                    TicksDelta = deltaClock.Restart().AsMicroseconds(),
                    EstimatedTicks = engineClock.ElapsedTime.AsMicroseconds()
                };
                UpdateEntities(tomasTime);

                DrawEntities();

                _window.Display();


                if (deltaClock.ElapsedTime.AsMicroseconds() < _targetMicrosec)
                {
                    int waitMsec = (int) (_targetMicrosec - deltaClock.ElapsedTime.AsMicroseconds()) / 1000;
                    if (waitMsec > 0)
                        Thread.Sleep(waitMsec);
                }

                //Thread.Sleep((int)(_targetMicrosec - deltaClock.ElapsedTime.AsMicroseconds()))
            }
        }

        private void InitEntities()
        {
            try
            {
                foreach (var entity in _entities)
                {
                    if (!entity.Initialized)
                        entity.Init(this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Entity initialization error! Exception: {0}", e.Message);
            }
        }

        private void UpdateEntities(TomasTime t)
        {
            foreach (var entity in _entities)
            {
                if(entity.Initialized)
                    entity.LogicUpdate(this, t);
            }
        }

        private void DrawEntities()
        {
            foreach (var entity in _entities)
            {
                if(entity.Initialized)
                    _window.Draw(entity);
            }
        }
    }
}