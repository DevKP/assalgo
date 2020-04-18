using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class ButtonArray : Transformable, IEntity
    {
        public bool Initialized { get; set; }
        public bool Visible { get; set; }
        private List<Button> _buttons;
        public void Init(TomasEngine o)
        {
            _buttons = new List<Button>();
            for(int x = 0; x < 10; x++)
                for(int y = 0; y < 10; y++)
                {
                    Button b = o.CreateEntity<Button>();
                    b.Size = new SFML.System.Vector2f(100, 50);
                    b.Position = new SFML.System.Vector2f(101 * x, 51 * y);
                    b.Init(o);
                    _buttons.Add(b);
                }
            Initialized = true;
        }
        public void Draw(RenderTarget target, RenderStates states)
        {

        }
        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {

        }
        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {

        }
    }
}
