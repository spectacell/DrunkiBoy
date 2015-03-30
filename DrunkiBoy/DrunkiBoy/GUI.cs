using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrunkiBoy
{
    class GUI
    {
        private string strLives, strLevel, strCurrentLevel, strItemsLeft, strTime, strTitle, strTitleVersion;
        private Vector2 strLivesPos = new Vector2(16, 16);
        private Vector2 healthBarPos = new Vector2(14, 57);
        private Vector2 strLevelPos = new Vector2(577, 16);
        private Vector2 strItemsLeftPos = new Vector2(920, 16);
        private Vector2 strTimePos = new Vector2(1032, 57);
        private Vector2 strTitlePos = new Vector2(843, 698);
        
        public GUI()
        {
            strLives = "LIVES ";
            strLevel = "LEVEL ";
            strItemsLeft = "ITEMS LEFT ";
            strTime = "TIME ";
            strTitle = "DRUNKIBOY ADVENTURES ";
            strTitleVersion = "V 1.0";
            int crntLevel = Game1.currentLevel + 1; //Första leveln är 0 så måste plussa på en för att visa rätt
            strCurrentLevel = crntLevel.ToString();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Constants.FONT_BIG, strLives, strLivesPos, Constants.fontColor);
            for (int i = 0; i < Player.livesLeft; i++)
            {
                spriteBatch.Draw(Textures.heart, new Vector2(strLivesPos.X + Constants.FONT_BIG.MeasureString(strLives).X + i*40, strLivesPos.Y+5), new Rectangle(0, 0, 31, 26), Color.White);
            }
            spriteBatch.Draw(Textures.healthBarRed, healthBarPos, Color.White);
            float greenBarWidth = (Player.healthLeft / Player.defaultHealth) * 260f;
            spriteBatch.Draw(Textures.healthBarGreen, new Vector2(healthBarPos.X + 3, healthBarPos.Y + 3), new Rectangle(0, 0, (int)greenBarWidth, 10), Color.White);
            spriteBatch.DrawString(Constants.FONT, strLevel, strLevelPos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strCurrentLevel, new Vector2(strLevelPos.X + Constants.FONT.MeasureString(strLevel).X, strLevelPos.Y), Constants.fontColor2);
            spriteBatch.DrawString(Constants.FONT_BIG, strItemsLeft, strItemsLeftPos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT_BIG, "3", new Vector2(strItemsLeftPos.X + Constants.FONT_BIG.MeasureString(strItemsLeft).X, strItemsLeftPos.Y), Constants.fontColor2);
            spriteBatch.DrawString(Constants.FONT_BIG, strTime, strTimePos, Constants.fontColor);
            int timeLeft = (int)Level.timeLeft; //För att avrunda till heltal
            spriteBatch.DrawString(Constants.FONT_BIG, timeLeft.ToString(), new Vector2(strTimePos.X + Constants.FONT_BIG.MeasureString(strTime).X, strTimePos.Y), Constants.fontColor2);
            spriteBatch.DrawString(Constants.FONT, strTitle, strTitlePos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strTitleVersion, new Vector2(strTitlePos.X + Constants.FONT.MeasureString(strTitle).X, strTitlePos.Y), Constants.fontColor2);
            spriteBatch.End();
        }
    }
}
