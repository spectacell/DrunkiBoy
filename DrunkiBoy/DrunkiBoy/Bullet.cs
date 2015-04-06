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
        private Vector2 velocity;
        private float skottRange;
        public Bullet(Vector2 pos,Vector2 velocity, Texture2D tex, bool isActive, float skottRange) : base(pos, tex, isActive)
        {
            this.velocity = velocity;
            this.skottRange = skottRange;
        }
        public void Update(GameTime gameTime)
        {                        
            pos = pos + velocity;

            skottRange = skottRange - velocity.Length();
            if (skottRange <= 0)
            {
                isActive = false;
            }

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
