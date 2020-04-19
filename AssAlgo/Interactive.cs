using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    enum MouseButtonState
    {
        Released,
        Pressed
    }

    abstract class Interactive : Transformable, IEntity
     {
        abstract public bool Visible { get; set; }
        abstract public bool Initialized { get; set; }

        private bool _hover;
        private bool _pressed;
        private bool _canDrag;
        private Vector2f _size;
        private TomasEngine _engine;

        private Vector2f LocalMouse;


        public EventHandler<MouseButtonEventArgs> OnMousePressed;
        public EventHandler<MouseButtonEventArgs> OnMouseReleased;

        public bool Dragging { get; protected set; }
        public bool Hover
        {
            get
            {
                var mCoords =_engine.ActiveWindow.MapPixelToCoords(_engine.MousePosition);
                if (PointInRectangle(new FloatRect(Position, Size), mCoords))
                    return true;
                return false;
            }
            protected set => _hover = value;
        }
        public bool Pressed 
        {
            get => _pressed;
            protected set => _pressed = value;
        }
        public bool CanDrag { get; set; }
        public Vector2f Size
        {
            get => _size;
            set
            {
                _size = value;
                Resized();
            }
        }

        abstract public int Z { get; set; }

        public Interactive(TomasEngine o)
        {
            _engine = o; ;
            o.MouseButtonPressed += (_, x) =>
            {
                var mCoords = o.ActiveWindow.MapPixelToCoords(o.MousePosition);
                LocalMouse = mCoords - Position;
                if (PointInRectangle(new FloatRect(Position, Size), mCoords))
                {
                    //Dragging = true;
                    _pressed = true;
                    OnMousePressed?.Invoke(this, x);
                    Clicked(mCoords - Position);
                }
            };
            o.MouseButtonReleased += (_, x) => 
            {
                //Dragging = false;
                OnMouseReleased?.Invoke(this, x);
                _pressed = false;
            };
        }
        protected bool PointInRectangle(FloatRect rect, Vector2f point)
        {
            return rect.Left < point.X && point.X < (rect.Left + rect.Width) &&
                rect.Top < point.Y && point.Y < (rect.Top + rect.Height);
        }

        abstract public void Resized();
        abstract public void Clicked(Vector2f localCoords);

        abstract public void Draw(RenderTarget target, RenderStates states);

        abstract public void Init(TomasEngine o);

        abstract public void LogicUpdate(TomasEngine engine, TomasTime time);

        abstract public void LogicUpdateAsync(TomasEngine engine, TomasTime time);
    }
}
