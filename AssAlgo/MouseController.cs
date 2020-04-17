using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    public class MouseController : Transformable, IEntity
    {
        public bool Initialized { get; set; }

        public bool Visible { get; set; }
        private CircleShape circleShape;
        private TestEntity te;

        public void Init(TomasEngine o)
        {
            circleShape = new CircleShape(20)
            {
                FillColor = Color.Yellow
            };

            o.MouseMoved += (_, a) 
                => te.Position = new Vector2f(a.X, a.Y);
            o.MouseButtonPressed += O_MouseButtonPressed;

            te = o.GetEntity<TestEntity>() as TestEntity;
            Initialized = true;
        }

        private void O_MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
        {
            var counter = new TestEntity
            {
                Position = new Vector2f(e.X,e.Y)
            };
            counter.Init((sender as TomasEngine));
            (sender as TomasEngine).HandleEntity(counter);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            target.Draw(circleShape, states);
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            
        }
    }
}