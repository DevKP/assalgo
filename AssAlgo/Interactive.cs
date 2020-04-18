using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
     abstract public class Interactive : Transformable, IEntity
     {
        abstract public bool Visible { get; set; }
        abstract public bool Initialized { get; set; }

        private Vector2f _size;
        private Vector2f _mouseOffset;

        public bool Dragging { get; protected set; }
        public bool Hover { get; protected set; }
        public bool Pressed { get; protected set; }
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

        public Interactive(TomasEngine o)
        {
           
            o.MouseButtonPressed += (_, x) =>
            {
                var mCoords = o.ActiveWindow.MapPixelToCoords(o.MousePosition);
                if (PointInRectangle(new FloatRect(Position, Size), mCoords))
                {
                    //Dragging = true;
                    Pressed = true;
                    _mouseOffset = Position - new Vector2f(x.X, x.Y);
                    Clicked(mCoords - Position);
                }
            };
            o.MouseMoved += (_, x) =>
            {
                var mCoords = o.ActiveWindow.MapPixelToCoords(o.MousePosition);
                if (PointInRectangle(new FloatRect(Position, Size), mCoords))
                {
                    Hover = true;
                    if (Dragging && CanDrag)
                    {
                        //Position = new Vector2f(mCoords.X + _mouseOffset.X, mCoords.Y + _mouseOffset.Y);
                    }
                }else
                {
                    Hover = false;
                }
            };
            o.MouseButtonReleased += (_, x) => 
            {
                //Dragging = false;
                Pressed = false;
            };
        }
        private bool PointInRectangle(FloatRect rect, Vector2f point)
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
