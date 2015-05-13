using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class BulletNote : Bullet
    {
        public BulletNote(Vector2 pos, Vector2 velocity, bool lethal)
            : base(pos, velocity, Textures.notesOne, true, 400)
        {
            this.lethal = lethal;
        }
    }
}
