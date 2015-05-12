using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Platform : GameObject
    {
        protected FireOnGround fire;
        protected int speed = 0;
        public Platform(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "platform"; //Bokstav eller namn som identifierar objektet i textfilen som läser in banan
        }
        public virtual void Update(GameTime gameTime, Player player)
        {
            if (fire != null)
            {
                fire.pos.X += speed;
                fire.Update(gameTime);
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (fire != null)
            {
                fire.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
        public void StartFire()
        {
            fire = new FireOnGround(new Vector2(pos.X + ((Textures.platform.Width - 240) / 2), pos.Y - Textures.fire.Height), Textures.fire, new Rectangle(0, 0, 240, 29), true, 4, 180);
        }
    }
   
}
