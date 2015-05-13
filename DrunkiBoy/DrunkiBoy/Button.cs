using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Button : GameObject
    {
        public enum buttontype { Button, ActiveButton };
        private buttontype currentButton;
        public bool isActivated;
        public Button(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "button";
        }
        public void activate()
        {
            isActivated = true;
            tex = Textures.ActiveButton;
        }
    }
}
