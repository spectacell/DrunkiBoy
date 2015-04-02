using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace DrunkiBoy
{
    // kan vara onödigt klass

    class Weapon : GameObject
    {
        public Weapon(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive)
            : base(pos, tex, isActive)
        {

        }
        public void Update(GameTime gameTime)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Textures.hamburgare, pos,srcRect, Color.White, 0.0f, Vector2.Zero, 0,SpriteEffects.None, 0);
        }
    }
}
