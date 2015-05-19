using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Sound
    {
        public static SoundEffect gameOver, hitWithShot, jump, pickUp, walkIntoWall, jp, shooting, bottleCrash, spawning, activatingToilet;
        public static SoundEffectInstance jetpack;
        public static Song song;
        public static void LoadContent(ContentManager content)
        {
            gameOver = content.Load<SoundEffect>("Sound/gameOver");
            hitWithShot = content.Load<SoundEffect>("Sound/hitWithShot");
            jump = content.Load<SoundEffect>("Sound/jump");
            pickUp = content.Load<SoundEffect>("Sound/pickUpStuff");
            walkIntoWall = content.Load<SoundEffect>("Sound/walkIntoWall");
            shooting = content.Load<SoundEffect>("Sound/shooting");
            bottleCrash = content.Load<SoundEffect>("Sound/bottleCrash");
            spawning = content.Load<SoundEffect>("Sound/spawn");
            activatingToilet = content.Load<SoundEffect>("Sound/activatingToilet");
            jp = content.Load<SoundEffect>("Sound/jetpack");
            jetpack = jp.CreateInstance();
            jetpack.IsLooped = true;
            song = content.Load<Song>("Sound/gameMusic");
        }
    }
   
}
