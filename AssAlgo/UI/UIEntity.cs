using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace AssAlgo
{
    enum MouseButtonState
    {
        Released,
        Pressed
    }

    abstract class UIEntity : Transformable, IEntity
    {
        public IEntity Parent { get; set; }

        abstract public bool Visible { get; set; }
        abstract public bool Initialized { get; set; }

        private int _z;
        private Vector2f _size;
        private Vector2f _localPosition;
        private TomasEngine _parentWorld;

        public bool Dragging { get; protected set; }
        public bool Hover { get; set; }
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

        public new Vector2f Position
        {
            get => _localPosition;
            set => _localPosition = value;
        }

        public Vector2f GlobalPosition
        {
            get => base.Position;
            set => base.Position = value;
        }

        public MouseButtonState mouseState;

        public int Z 
        {
            get => _z;
            set
            {
                _z = value;
                if(Parent != null)
                    (Parent as UIEntity).Z = _z - 1;
                _parentWorld.ZInvalidate();
            }
        }

        public UIEntity(TomasEngine o, IEntity parent)
        {
            _parentWorld = o;
            this.Parent = parent;
            this._z = parent?.Z + 1 ?? 1;
        }
        protected bool PointInRectangle(FloatRect rect, Vector2f point)
        {
            return rect.Left < point.X && point.X < (rect.Left + rect.Width) &&
                rect.Top < point.Y && point.Y < (rect.Top + rect.Height);
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            if (Parent != null)
            {
                this.GlobalPosition = (Parent as UIEntity).GlobalPosition + this.Position;
                this._z = Parent.Z + 1;
            }
            else
                this.GlobalPosition = this.Position;
            this.UIUpdate(engine, time);
        }

        public virtual void OnEntityMouseDown(TomasEngine engine, MouseButtonEventArgs a) { }
        public virtual void OnEntityMouseUp(TomasEngine engine, MouseButtonEventArgs a) { }
        public virtual void OnEntityMouseMoved(TomasEngine engine, MouseMoveEventArgs a) { }

        abstract public void Resized();

        abstract public void Draw(RenderTarget target, RenderStates states);

        abstract public void Init(TomasEngine o);

        abstract public void UIUpdate(TomasEngine engine, TomasTime time);

        abstract public void LogicUpdateAsync(TomasEngine engine, TomasTime time);
    }
}
