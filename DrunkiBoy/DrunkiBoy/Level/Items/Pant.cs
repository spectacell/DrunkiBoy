﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Pant : GameObject
    {
        ParticleEngine particleEngine;
        bool moving;      
        public Pant(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive) : base(pos, tex, isActive)
        {
            this.type = "pant";
            particleEngine = new ParticleEngine(Textures.pantParticles, pos, false);
        }
        public void PickUp()
        {            
            moving = true;
            particleEngine.isActive = true;
        }
        public void Update(GameTime gameTime)
        {
            if (moving)
            {
                particleEngine.Update(pos);
                if (pos.Y > -2000)
                {
                    pos.Y -= 15;
                }
                else
                {
                    isActive = false;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (particleEngine.isActive)
            {
                particleEngine.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
    }
}
