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
        public bool isActivated;
        public Toilet(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "toilet";
        }
    }
}
