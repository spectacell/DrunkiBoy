using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class MovingPlatform :Platform
    {
        Vector2 startpos;
        Vector2 endpos;
        int speed = 1;

        public MovingPlatform(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            startpos.X = pos.X;
            endpos.X = startpos.X + 100;
            this.type = "movingplatform"; //Bokstav eller namn som identifierar objektet i textfilen som läser in banan
        }
        public void Update(Player player)
        {
            pos.X += speed;
            if (player.activePlatform == this)
            {
                player.pos.X += speed;
            }
            if ((pos.X >= endpos.X) || (pos.X <= startpos.X))
            {
                speed *= -1;
            }
        }

    }
}
