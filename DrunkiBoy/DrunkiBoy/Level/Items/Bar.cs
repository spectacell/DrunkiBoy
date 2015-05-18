using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Bar : GameObject
    {
        public bool choseBurgers, choseKebab, choseBottles, hasBought;
        public Bar(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "bar";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(choseBurgers)
                spriteBatch.Draw(Textures.barWithoutBurgers, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            else if (choseKebab)
                spriteBatch.Draw(Textures.barWithoutKebab, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            else if (choseBottles)
                spriteBatch.Draw(Textures.barWithoutBottles, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            else
            base.Draw(spriteBatch);
        }
    }
}
