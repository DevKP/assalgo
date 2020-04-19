using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace AssAlgo
{
    class TomasText : Transformable, IEntity
    {
        public bool Initialized { get; set; }
        public bool Visible { get; set; } = true;


        private Text _text;

        public string Text
        {
            get => _text.DisplayedString;
            set => _text.DisplayedString = value;
        }

        public uint CharacterSize 
        { 
            get => _text.CharacterSize;
            set => _text.CharacterSize = value;
        }
        public int Z { get; set; } = 0;

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Visible)
            {
                states.Transform *= Transform;
                target.Draw(_text, states);
            }
        }

        public void Init(TomasEngine o)
        {
            _text = new Text("", o.opensense, 8);
            _text.FillColor = Color.White;
            Initialized = true;
        }

        public void LogicUpdate(TomasEngine engine, TomasTime time)
        {
            
        }

        public void LogicUpdateAsync(TomasEngine engine, TomasTime time)
        {
            
        }
    }
}
