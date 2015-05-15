using Microsoft.Xna.Framework;
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
        double shotTimer, resetTimer, gt;
        Random rnd;
        int rndTex, rndDir;
        Texture2D noteTex;
        float dir;

        public Radio(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "radio";
            resetTimer = 75;
            shotTimer = 75;
            health = Constants.health_radio;
            rnd = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            gt = gameTime.TotalGameTime.TotalSeconds;
            shotTimer--;
            if (shotTimer <= 0)
            {
                rndTex = rnd.Next(0, 4);

                if (rndTex == 0)
                    noteTex = Textures.notesOne;
                else if (rndTex == 1)
                    noteTex = Textures.notesTwo;
                else if (rndTex == 2)
                    noteTex = Textures.notesThree;
                else if (rndTex == 3)
                    noteTex = Textures.notesFour;

                rndDir = rnd.Next(0, 2);
                if (rndDir == 0)
                    dir = 3;
                else if (rndDir == 1)
                    dir = -3;

                bulletNote = new BulletNote(pos, noteTex, new Rectangle(0, 0, 50, 50), true, new Vector2(dir, 0));
                ItemManager.AddBulletNotes(bulletNote);
                shotTimer = resetTimer;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
