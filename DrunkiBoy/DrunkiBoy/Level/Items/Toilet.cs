using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    class Toilet : GameObject
    {
        private ParticleEngine2 particleEngine = new ParticleEngine2(Textures.smokeParticles, Vector2.Zero, 2, 1, Textures.bubble_particle, false);
        public bool isActivated, isCurrentSpawn;
        public Toilet(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "toilet";
            particleEngine = new ParticleEngine2(Textures.bubble_particle, new Vector2(pos.X + 32, pos.Y + srcRect.Height - 45), 10, 1, Textures.water_texture, false);
        }
        public void Update(Player player)
        {
            if (isCurrentSpawn && player.spawning)
            {
                particleEngine.isActive = true;
                
                particleEngine.height += (float)Math.Pow(0.1,2);
                particleEngine.width += 0.1f;
            }
            else
            {
                particleEngine.isActive = false;
            }
            particleEngine.Update();
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            particleEngine.Draw(spriteBatch);
        }
    }
}
