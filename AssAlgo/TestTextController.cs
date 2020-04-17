using SFML.Graphics;
using SFML.System;

namespace AssAlgo
{
    public class TestTextController : Transformable, IEntity
    {
        public bool Initialized { get; set; }
        public bool Visible { get; set; }

        private TestEntity testEntity;
        public void Draw(RenderTarget target, RenderStates states)
        {
            
        }

        public void Init(TomasEngine engine)
        {
            testEntity = engine.GetEntity<TestEntity>() as TestEntity;
            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            //float moveamount = (float)time.TicksDelta / Time.FromSeconds(5).AsMicroseconds() * 700f;
            //testEntity.Position += new Vector2f(moveamount, moveamount);
        }
    }
}