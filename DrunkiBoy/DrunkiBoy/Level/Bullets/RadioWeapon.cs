using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class HamburgareVapen : Bullet
    {
        ParticleEngine particleEngine;
        bool shot;
        public HamburgareVapen(Vector2 pos, Vector2 velocity, bool lethal)
            : base(pos, velocity, Textures.hamburgare, true, 400)    // sätt texturen till hamburgare , 200 för hur långt det åker
        {
            particleEngine = new ParticleEngine(Textures.burgerParticles, pos, false);
            this.lethal = lethal;
        }
    }
}
