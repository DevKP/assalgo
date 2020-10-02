using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using AssAlgo.UI;

namespace AssAlgo
{

    public class TomasEngine
    {
        private List<IEntity> _entities;
        private bool _zOrderValid;
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

        public event EventHandler<SizeEventArgs> OnResized;

        public int EntityNumber => _entities.Count;

        public Color ClearColor { get; set; }

        public int TargetFramerate
        {
            get => _targetFramerate;
            set
            {
                _targetFramerate = value;
                _targetMicrosec = Time.FromSeconds(1).AsMicroseconds() / _targetFramerate;
            }
        }

        public RenderWindow ActiveWindow { get; set; }

        public Vector2i MousePosition => _mousePos;

        public Vector2u WindowsSize => ActiveWindow.Size;

        public TomasEngine(string title, uint width, uint height, VideoMode mode)
        {
            ActiveWindow = new RenderWindow(mode, title, Styles.Default, new ContextSettings() { AntialiasingLevel = 1 });
            ActiveWindow.SetVisible(false);
            ActiveWindow.Size = new Vector2u(width, height);
            ActiveWindow.SetView(new View(new FloatRect(0f, 0f, width, height)));

            //////////////
            ActiveWindow.Closed += (o, _) => ActiveWindow.Close();
            ActiveWindow.Resized += (_, x) =>
            {
                ActiveWindow.SetView(new View(new FloatRect(0f, 0f, x.Width, x.Height)));
                _windowsSize = new Vector2u(x.Width, x.Height);
                OnResized?.Invoke(this, x);
            };
            ActiveWindow.MouseButtonPressed += DisposeMousePressed;
            ActiveWindow.MouseMoved += (o, a) =>
            {
                _mousePos = new Vector2i(a.X, a.Y);
            };
            ActiveWindow.MouseMoved += DisposeMouseMoved;
            ActiveWindow.MouseButtonReleased += DisposeMouseReleased;
            ActiveWindow.MouseWheelScrolled += (o, a) => MouseWheelScrolled?.Invoke(this, a);

            ActiveWindow.KeyPressed += DisposeKeyPressed;
            ActiveWindow.KeyReleased += DisposeKeyPressed;
            //////////////

            ClearColor = new Color(45, 45, 45);
            TargetFramerate = 120;

            _entities = new List<IEntity>();
        }

        private void DisposeKeyPressed(object sender, KeyEventArgs a)
        {
            var descendEntities = _entities.OrderByDescending(e => e.Z);
            foreach (var entity in descendEntities)
            {
                if (!(entity is UIEntity)) continue;

                var uiEntity = entity as UIEntity;
                if (uiEntity.Visible)
                    uiEntity.KeyboardState = a.Shift ? KeyboardButtonState.Shift : KeyboardButtonState.None;
            }
        }

        private void DisposeMousePressed(object sender, MouseButtonEventArgs a)
        {
            var descendEntities = _entities.OrderByDescending(e => e.Z);
            foreach (var entity in descendEntities)
            {
                if (!(entity is UIEntity)) continue;
                //(entity as UIEntity).OnEntityMouseDown(this, a);
                var uiEntity = entity as UIEntity;
                if (uiEntity.Visible && PointInRectangle(new FloatRect(uiEntity.GlobalPosition, uiEntity.Size), new Vector2f(a.X,a.Y)))
                {
                    (entity as UIEntity).OnEntityMouseDown(this, a);
                    (entity as UIEntity).MouseState = MouseButtonState.Pressed;
                    break;
                }
            }
        }

        private void DisposeMouseReleased(object sender, MouseButtonEventArgs a)
        {
            var descendEntities = _entities.OrderByDescending(e => e.Z);
            foreach (var entity in descendEntities)
            {
                if (entity is UIEntity)
                {
                    UIEntity uiEntity = entity as UIEntity;
                    uiEntity.OnEntityMouseUp(this, a);
                    (entity as UIEntity).MouseState = MouseButtonState.Released;
                }
            }
        }

        private void DisposeMouseMoved(object sender, MouseMoveEventArgs a)
        {
            bool sended = false;
            for (int i = 0; i < _entities.Count; i++)
            {
                var entity = _entities[_entities.Count - i - 1];
                if (entity is UIEntity)
                {
                    UIEntity uiEntity = entity as UIEntity;
                    if (uiEntity.Visible && PointInRectangle(new FloatRect(uiEntity.GlobalPosition, uiEntity.Size), new Vector2f(a.X, a.Y)) && !sended)
                    {
                        uiEntity.OnEntityMouseMoved(this, a);
                        uiEntity.Hover = true;
                        sended = true;
                    }
                    else
                        (entity as UIEntity).Hover = false;
                }
            }
        }

        private bool PointInRectangle(FloatRect rect, Vector2f point)
        {
            return rect.Left < point.X && point.X < (rect.Left + rect.Width) &&
                rect.Top < point.Y && point.Y < (rect.Top + rect.Height);
        }

        public void ZInvalidate()
        {
            _zOrderValid = false;
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
        public T CreateEntity<T, P>(P parent)
            where T : IEntity
            where P : IEntity?
        {
            Type[] types = { typeof(TomasEngine),
                            typeof(P)};
            T entity;
            if (typeof(T).GetConstructor(types) != null)
                entity = (T)Activator.CreateInstance(typeof(T), this, parent);
            else
                entity = (T)Activator.CreateInstance(typeof(T));

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

            ActiveWindow.SetVisible(true);
            ActiveWindow.SetFramerateLimit(120);

            Clock engineClock = new Clock();
            Clock deltaClock = new Clock();
            Clock debugUpdateClock = new Clock();
            Clock debugDrawClock = new Clock();

            TomasTime tomasTime = new TomasTime();

            long frameCount = 0;

            while (ActiveWindow.IsOpen)
            {
                if (frameCount % 2 == 0)
                    ActiveWindow.DispatchEvents();

                ActiveWindow.Clear(ClearColor);
                debugUpdateClock.Restart();

                tomasTime.TicksDelta = deltaClock.Restart().AsMicroseconds();
                tomasTime.EstimatedTicks = engineClock.ElapsedTime.AsMicroseconds();

                UpdateEntities(tomasTime);
                tomasTime.LastUpdateTime = debugUpdateClock.Restart().AsMicroseconds();

                debugDrawClock.Restart();
                DrawEntities();
                tomasTime.LastDrawTime = debugDrawClock.Restart().AsMicroseconds();

                ActiveWindow.Display();
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
            if (_zOrderValid == false)
            {
                _entities = _entities.OrderBy(e => e.Z).ToList();
                _zOrderValid = true;
            }

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
            _entities.ForEach(e => ActiveWindow.Draw(e));
        }
    }
}