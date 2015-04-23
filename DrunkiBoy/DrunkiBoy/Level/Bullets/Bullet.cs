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
        protected Vector2 velocity, origin;
        protected float shotRange, rotation;
        ParticleEngine particleEngine;
        public Bullet(Vector2 pos,Vector2 velocity, Texture2D tex, bool isActive, float shotRange) : 
            base(pos, tex, isActive)
        {
            this.velocity = velocity;
            this.shotRange = shotRange;
            this.origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }
        public virtual void Update(GameTime gameTime)
        {                        
            pos += velocity;
            velocity.Y += 0.1f; //gravitation...
            rotation += 0.1f;
            shotRange = shotRange - velocity.Length();
            
            if (shotRange <= 0)
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
