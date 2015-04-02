using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    //här är jag  osäker om vi ska ärva från gameobject eller hur jag ska fortsätta
    //behövs Update Draw method  Update 
    class Bullet  : GameObject
    {

        public Bullet(Vector2 pos, Texture2D tex, bool isActive) : base(pos, tex, isActive)
        {

        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
