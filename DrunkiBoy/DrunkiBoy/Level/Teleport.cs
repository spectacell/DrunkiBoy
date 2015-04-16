using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    class Teleport : AnimatedObject
    {   
        // ska vi adda här switch mellan aktiv och ej aktiv teleport?
        bool isActive;
        public enum teleportType{teleport, aktivTeleport};
        private teleportType currentTeleport;
         public Teleport(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            
            this.type = "teleport";            
           
        }
       
        public void activate()
        {
            isActive = true;
            tex = Textures.AktivTeleport;
        }
    }
}
