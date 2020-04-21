using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo.Engine
{
    public enum AnimationType
    {
        Linear,
        Cos
    }
    public class Animator
    {
        private AnimationType _type;
        private float _x;
        private int _duration;

        public float Position { get; private set; } //[0..1]
        public bool Completed { get; private set; } = true;

        public Animator(int duration, AnimationType type = AnimationType.Linear)
        {
            _duration = duration;
            _type = type;
            _x = 0;
        }
        public float Step(int timeDelta)
        {
            var dx = (float)timeDelta / (float)_duration;
            if (_x + dx <= 1f && !Completed)
            {
                _x += dx;
                var posDelta = (-MathF.Cos(_x * MathF.PI) / 2f + 0.5f) - Position;
                Position += posDelta;
                return posDelta;
            }
            else
            {
                Completed = true;
                return 0f;
            }
        }
        public void Reset()
        {
            _x = 0;
            Completed = false;
            Position = 0f;
        }
    }
}
