using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private Vector2i _mousePos;

        public Font arial = new Font("Arial.ttf");
        public Font opensense = new Font("OpenSans-Light.ttf");
        public Font opensense_reg = new Font("OpenSans-Regular.ttf");

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
        //public event EventHandler<MouseMoveEventArgs> MouseMoved = delegate { };

        public int EntityNumber { get => _entities.Count; }

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

        public RenderWindow ActiveWindow 
        {
            get => _window;
            set => _window = value;
        }

        public Vector2i MousePosition
        {
            get => _mousePos;
        }

        public Vector2u WindowsSize => _window.Size;

        public TomasEngine(string title, uint width, uint height, VideoMode mode)
        {
            _window = new RenderWindow(mode, title,Styles.Default, new ContextSettings() {AntialiasingLevel = 1 });
            _window.SetVisible(false);
            _window.Size = new Vector2u(width,height);
            _window.SetView(new View(new FloatRect(0f,0f, width, height)));

            //////////////
            _window.Closed += (o, _) => _window.Close();
            _window.Resized += (_, x) =>
            {
                _window.SetView(new View(new FloatRect(0f, 0f, x.Width, x.Height)));
                _windowsSize = new Vector2u(x.Width, x.Height);
            };
            _window.MouseButtonPressed += (o, a) => MouseButtonPressed?.Invoke(this, a);
            _window.MouseMoved += (o, a) =>
            {
                _mousePos = new Vector2i(a.X, a.Y);
            };
            _window.MouseButtonReleased += (o, a) => MouseButtonReleased?.Invoke(this, a);
            _window.MouseWheelScrolled += (o, a) => MouseWheelScrolled?.Invoke(this, a);
            //////////////

            _clearColor = new Color(45,45,45);
            TargetFramerate = 120;

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
            entity.Init(this);
            _entities.Add(entity);
            return _entities.Count;
        }
        public T CreateEntity<T>() where T: IEntity
        {
            Type[] types = { typeof(TomasEngine) };
            IEntity entity;
            if (typeof(T).GetConstructor(types) != null)
                entity = Activator.CreateInstance(typeof(T), this) as IEntity;
            else
                entity = Activator.CreateInstance(typeof(T)) as IEntity;

            entity.Init(this);
            _entities.Add(entity);
            _entities = _entities.OrderBy(e => e.Z).ToList();
            return (T)entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            _entities.Remove(entity);
        }

        public void Run()
        {
            //InitEntities();

            _window.SetVisible(true);
            _window.SetFramerateLimit(120);

            Clock engineClock = new Clock();
            Clock deltaClock = new Clock();
            Clock debugUpdateClock = new Clock();
            Clock debugDrawClock = new Clock();

            TomasTime tomasTime = new TomasTime();

            long frameCount = 0;

            while (_window.IsOpen)
            {
                if(frameCount % 2 == 0)
                    _window.DispatchEvents();

                _window.Clear(_clearColor);

                tomasTime.TicksDelta = deltaClock.Restart().AsMicroseconds();
                tomasTime.EstimatedTicks = engineClock.ElapsedTime.AsMicroseconds();

                debugUpdateClock.Restart();
                UpdateEntities(tomasTime);
                tomasTime.LastUpdateTime = debugUpdateClock.Restart().AsMicroseconds();

                debugDrawClock.Restart();
                DrawEntities();
                tomasTime.LastDrawTime = debugDrawClock.Restart().AsMicroseconds();

                _window.Display();
                frameCount += 1;
                //if (deltaClock.ElapsedTime.AsMicroseconds() < _targetMicrosec)
                //{
                //    int waitMsec = (int) (_targetMicrosec - deltaClock.ElapsedTime.AsMicroseconds()) / 1000;
                //    if (waitMsec > 0)
                //        Thread.Sleep(waitMsec);
                //}

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
                if (entity.Initialized)
                {
                    entity.LogicUpdate(this, t);
                    //Task.Run(() => entity.LogicUpdateAsync(this, t));
                }
            }
        }

        private void DrawEntities()
        {
            //foreach (var entity in _entities)
            //{
            //    _window.Draw(entity);

            //}
            _entities.ForEach(e => _window.Draw(e));
        }
    }
}