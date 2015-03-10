using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Så här ser en ny klass av ett statiskt objekt ut
namespace DrunkiBoy.Mall
{
    class Mall_StatisktObjekt : GameObject
    {
        public Mall_StatisktObjekt(Vector2 pos, Texture2D tex, bool isActive)
            : base(pos, tex, isActive)
        {
            this.type = "namn"; //Skriv ett namn eller en bokstav här som identifierar klassen. Används i .txt-filerna där banorna sparas och läses in från
        }
    }
}