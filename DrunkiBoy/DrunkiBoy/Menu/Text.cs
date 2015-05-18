using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DrunkiBoy
{
    class Text
    {
        SpriteFont spriteFont;

        public string text;
        public Vector2 position;

        public Text(SpriteFont spriteFont, string text, Vector2 position)
        {
            this.spriteFont = spriteFont;
            this.text = text;
            this.position = position;
        }

        public void DrawClickableText(SpriteBatch spriteBatch, Color color)
        {
            if (KeyMouseReader.mouseState.X > position.X && KeyMouseReader.mouseState.X < spriteFont.MeasureString(text).X + position.X &&
                KeyMouseReader.mouseState.Y > position.Y && KeyMouseReader.mouseState.Y < spriteFont.MeasureString(text).Y + position.Y)
            {
                color = Color.Green;
            }
            spriteBatch.DrawString(spriteFont, text, position, color, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
       
        public bool IsClicked()
        {
            if (KeyMouseReader.LeftClick())
            {
                if (KeyMouseReader.mouseState.X > position.X && KeyMouseReader.mouseState.X < spriteFont.MeasureString(text).X + position.X &&
                KeyMouseReader.mouseState.Y > position.Y && KeyMouseReader.mouseState.Y < spriteFont.MeasureString(text).Y + position.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
