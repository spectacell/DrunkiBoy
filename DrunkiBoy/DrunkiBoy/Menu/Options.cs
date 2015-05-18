using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;


namespace DrunkiBoy
{
    class Options
    {
        AudioEngine audioEngine;
        SoundBank soundBank;
        WaveBank waveBank;
        AudioCategory musicCategory;

        public Options()
        {

        }

        public void Update(GameTime gametime)
        {
            audioEngine.Update();
        }


    }

}
