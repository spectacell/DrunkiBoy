using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class AngryNeighbour : Enemy
    {
        private int speed = 70;

        public AngryNeighbour(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "angryNeighbour";

            health = 2;
        }
        public override void Update(GameTime gameTime)
        {
            pos.X += (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
        }      
    }
}