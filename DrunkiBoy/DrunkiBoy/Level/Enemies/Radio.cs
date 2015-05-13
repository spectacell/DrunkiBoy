﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Radio : Enemy
    {
        private ParticleEngine2 pE;
        BulletNote bulletNote;
        public Radio(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "radio";
            //pE = new ParticleEngine2(Textures.radio, new Vector2(pos.X, pos.Y), Textures.radio, true);

            health = Constants.health_radio;
        }
        public override void Update(GameTime gameTime)
        {
            //pE.Update();
            //bulletNote = new BulletNote(pos, new Vector2(20, 0), false);
            //BulletManager.AddBullet(bulletNote);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //pE.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
