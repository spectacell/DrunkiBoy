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
        public enum teleportType{teleport, aktivTeleport};
        private teleportType currentTeleport;
         public Teleport(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive, int nrFrames, double frameInterval)
            : base(pos, tex, srcRect, isActive, nrFrames, frameInterval)
        {
            
            this.type = "teleport";            
           
        }
        public void isTeleportAktiv(teleportType type)
         {
             switch (type)
             {
                 case teleportType.teleport:
                     tex = Textures.teleport;
                     
                     break;
                 case teleportType.aktivTeleport:
                     tex = Textures.AktivTeleport;
                     break;
                 default:
                     break;
             }
         }
    }
}
