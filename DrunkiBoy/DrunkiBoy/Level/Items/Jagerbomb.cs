using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Jagerbomb : GameObject
    {
        private ParticleEngine2 pE;
        public Jagerbomb(Vector2 pos, Texture2D tex, bool isActive)
                : base(pos, tex, isActive)
        {
            this.type = "jagerbomb"; 
            pE = new ParticleEngine2(Textures.bubble_particle, new Vector2(pos.X + 10, pos.Y + 15), 4, 3, Textures.jagerbomb, true);
        }
        public void Update(GameTime gameTime)
        {
            pE.Update();
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            pE.Draw(spriteBatch);
            spriteBatch.Draw(tex, pos, srcRect, Color.White*0.8f, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
        }
    }
}
