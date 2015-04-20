using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DrunkiBoy
{
    abstract class GameObject
    {
        public Vector2 pos;
        public Texture2D tex;
        public string type; //Bokstav eller namn som identifierar objektet i textfilen som läser in banan
        public bool isActive; // Kan behövas om objektet ska markeras för bortplockning ur en lista eller om det inte ska ritas ut alls. 
        public Rectangle srcRect; //Rektangeln i spritesheeten där bilden är.
        public float drawLayer; // För spriteBatch.Draw så att vi kan styra vad som ritas ut ovanför vad.
        protected Color[] colorData; //För pixelkollision
        public GameObject(Vector2 pos, Texture2D tex, Rectangle srcRect, bool isActive)
        {
            this.pos = pos;
            this.srcRect = srcRect;
            this.isActive = isActive;
            this.tex = tex;
            SetColorData();
        }
        public GameObject(Vector2 pos, Texture2D tex, bool isActive) //Utan srcRect för de objekt som bara består av en bild
        {
            this.pos = pos;
            this.isActive = isActive;
            this.tex = tex;
            srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            SetColorData();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (isActive) 
            { 
                spriteBatch.Draw(tex, pos, srcRect, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, drawLayer);
            }
        }
        /// <summary>
        /// Returnerar en BB för hela objektet.
        /// </summary>
        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)pos.X, (int)pos.Y, srcRect.Width, srcRect.Height);
            }
        }
        /// <summary>
        /// Returnerar en BB vid objektets övre del
        /// </summary>
        public virtual Rectangle TopBoundingBox
        {
            get
            {
                return new Rectangle((int)pos.X, (int)pos.Y, srcRect.Width, 20);
            }
        }
        /// <summary>
        /// Returnerar en BB vid objekets nedre del. 
        /// </summary>
        public Rectangle BottomBoundingBox
        {
            get
            {   //Har joxat lite med bredden här för att det skulle se bra ut med att hoppa upp o ner från plattformar. Kanske behöver ändras sen.
                return new Rectangle((int)pos.X+8, (int)pos.Y + srcRect.Height - 5, srcRect.Width-16, 5);
            }
        }
        /// <summary>
        /// Gets the color data of the SpriteSheet
        /// </summary>
        public void SetColorData()
        {
            colorData = new Color[tex.Width * tex.Height];
            tex.GetData(colorData);
        }
        /// <summary>
        /// Hittar pixel på samma relativa position i båda objektens colorData Array...typ
        /// </summary>
        /// <param name="col">kolumn</param>
        /// <param name="row">rad</param>
        /// <returns></returns>
        public Color GetPixel(int col, int row)
        {
            int c = col - BoundingBox.X + srcRect.X;
            int r = row - BoundingBox.Y + srcRect.Y;
            return colorData[r * tex.Width + c];
        }
        /// <summary>
        /// Kollar först om BB intersektar, tar sedan reda på var det överlappar och kollar igenom pixel för pixel tills samma position i båda objekten innehåller en färg
        /// </summary>
        /// <param name="go">GameObject</param>
        /// <returns></returns>
        public bool DetectPixelCollision(GameObject go)
        {
            if (!BoundingBox.Intersects(go.BoundingBox))
            {
                return false;
            }
            int top = Math.Max(BoundingBox.Top, go.BoundingBox.Top);
            int bottom = Math.Min(BoundingBox.Bottom, go.BoundingBox.Bottom);
            int left = Math.Max(BoundingBox.Left, go.BoundingBox.Left);
            int right = Math.Min(BoundingBox.Right, go.BoundingBox.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    if (GetPixel(x, y).A > 0 && go.GetPixel(x, y).A > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void DeactivateIfOutOfBounds(Rectangle bounds)
        {
            if (pos.X < -srcRect.Width || pos.X > bounds.Width || pos.Y < -srcRect.Height || pos.Y > bounds.Height)
            {
                isActive = false;
            }
        }
    }
}
