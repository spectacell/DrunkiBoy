﻿using System;
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
        private Vector2 velocity, origin;
        private float skottRange, rotation;
        public Bullet(Vector2 pos,Vector2 velocity, Texture2D tex, bool isActive, float skottRange) : 
            base(pos, tex, isActive)
        {
            this.velocity = velocity;
            this.skottRange = skottRange;
            this.origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }
        public override void Update(GameTime gameTime)
        {                        
            pos += velocity;
            pos.Y += 0.6f; //gravitation...
            rotation += 0.1f;
            skottRange = skottRange - velocity.Length();
            
            if (skottRange <= 0)
            {
                isActive = false;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos+origin, srcRect, Color.White, rotation, origin, 1f, SpriteEffects.None, drawLayer);
        }
    }
}
