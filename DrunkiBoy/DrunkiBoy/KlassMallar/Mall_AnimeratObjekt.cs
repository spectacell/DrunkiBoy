using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//En mall som visar hur en ny klass med ett animerat objekt ser ut.
namespace DrunkiBoy.Mall 
{
    class Mall_AnimeratObjekt : AnimatedObject
    {
        public Mall_AnimeratObjekt(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            this.type = "namn"; //Skriv ett namn eller en bokstav här som identifierar klassen. Används i .txt-filerna där banorna sparas och läses in från
        }
    }
}
