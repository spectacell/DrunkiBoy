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
        public bool isActivated;
        public enum teleportType{teleport, aktivTeleport};
        private teleportType currentTeleport;
         public Teleport(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            
            this.type = "teleport";            
           
        }
       
        public void activate()
        {
            isActivated = true;
            tex = Textures.AktivTeleport;
        }
    }
}
