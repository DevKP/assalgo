using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace AssAlgo.UI
{
    internal enum MouseButtonState
    {
        Released,
        Pressed
    }

    internal enum KeyboardButtonState
    {
        None,
        Shift
    }

    abstract class UIEntity : Transformable, IEntity
    {
        public IEntity Parent { get; set; }

        public abstract bool Visible { get; set; }
        public abstract bool Initialized { get; set; }

        private int _z;
        private Vector2f _size;
        private readonly TomasEngine _parentWorld;

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

        public new Vector2f Position { get; set; }

        public Vector2f GlobalPosition
        {
            get => base.Position;
            set => base.Position = value;
        }

        public MouseButtonState MouseState;
        public KeyboardButtonState KeyboardState;

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

        protected UIEntity(TomasEngine o, IEntity parent)
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

        public abstract void Resized();

        public abstract void Draw(RenderTarget target, RenderStates states);

        public abstract void Init(TomasEngine o);

        public abstract void UIUpdate(TomasEngine engine, TomasTime time);

        public abstract void LogicUpdateAsync(TomasEngine engine, TomasTime time);
    }
}
