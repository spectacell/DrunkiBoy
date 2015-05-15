using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class BulletNote : GameObject
    {
        Vector2 direction;
        Vector2 startPos;
        public BulletNote(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, Vector2 direction)
            : base(pos, tex, srcRect, isActive)
        {
            this.type = "bulletNote";
            this.direction = direction;
            startPos = pos;
        }

        public void Update(GameTime gameTime)
        {
            pos += direction;

            if (pos.X >= startPos.X + 500)
                isActive = false;
            if (pos.X <= startPos.X - 500)
                isActive = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
