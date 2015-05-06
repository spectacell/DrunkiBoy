using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace DrunkiBoy
{
    public class GUI
    {
        private string strLives, strLevel, strCurrentLevel, strItemsLeft, strTime, strTitle, strTitleVersion;
        private Vector2 strLivesPos, healthBarPos, strLevelPos, strItemsLeftPos, strTimePos, strTitlePos, powerUpPos;
        private int scorePosY;
        private ActivePowerUpDisplay activePowerUp;
        private Texture2D healthBarTex;
        private double healthBarBlinkTimer, healthBarBlinkTimerDefault = 250;
        public static bool healthBarBlinking;
        public static int itemsLeftToCollect;

        private float burgerOpacity, kebabOpacity, bottleOpacity, PizzaOpacity;
        private Color burgerColor, kebabColor, bottleColor, pizzaColor;

        public GUI()
        {
            strLives = "LIVES ";
            strLevel = "LEVEL ";
            strItemsLeft = "ITEMS LEFT ";
            strTime = "TIME ";
            strTitle = "DRUNKIBOY ADVENTURES ";
            strTitleVersion = "V 2.0";
            int crntLevel = Game1.currentLevel + 1; //Första leveln är 0 så måste plussa på en för att visa rätt
            strCurrentLevel = crntLevel.ToString();

            strLivesPos = new Vector2(16, 16);
            healthBarPos = new Vector2(13, 57);
            strLevelPos = new Vector2(577, 16);
            scorePosY = 57;
            strItemsLeftPos = new Vector2(920, 16);
            strTimePos = new Vector2(1032, 57);
            strTitlePos = new Vector2(833, 698);
            
            powerUpPos = new Vector2(Textures.healthBarRed.Width+25, strLivesPos.Y + 3);
            activePowerUp = new ActivePowerUpDisplay(powerUpPos, Textures.powerUpTimer, new Rectangle(0,0,63,63), false, 13, 1000, 0);

            healthBarTex = Textures.healthBarRed;

        }

        public virtual void Update(GameTime gameTime)
        {
            activePowerUp.Update(gameTime);
            HealthBarBlinking(gameTime);
            BlinkHealthBar();
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //Liv kvar
            spriteBatch.DrawString(Constants.FONT_BIG, strLives, strLivesPos, Constants.fontColor);
            for (int i = 0; i < Player.livesLeft; i++)
            {
                spriteBatch.Draw(Textures.heart, new Vector2(strLivesPos.X + Constants.FONT_BIG.MeasureString(strLives).X + i*40, strLivesPos.Y+5), new Rectangle(0, 0, 31, 26), Color.White);
            }
            //Health bar
            spriteBatch.Draw(healthBarTex, healthBarPos, Color.White);
            float greenBarWidth = ((float)Player.healthLeft / (float)Constants.player_defaultHealth) * Textures.healthBarGreen.Width;
            spriteBatch.Draw(Textures.healthBarGreen, new Vector2(healthBarPos.X + 3, healthBarPos.Y + 3), new Rectangle(0, 0, (int)greenBarWidth, Textures.healthBarGreen.Height), Color.White);
            //Aktiv powerup
            activePowerUp.Draw(spriteBatch);
            //Level
            float centerPosLevel = Game1.windowWidth / 2 - Constants.FONT.MeasureString(strLevel + strCurrentLevel).X / 2;
            spriteBatch.DrawString(Constants.FONT, strLevel, new Vector2(centerPosLevel, strLevelPos.Y), Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strCurrentLevel, new Vector2(centerPosLevel + Constants.FONT.MeasureString(strLevel).X, strLevelPos.Y), Constants.fontColor2);
            //Poäng
            float centerPosScore = Game1.windowWidth/2-Constants.FONT.MeasureString(Player.score.ToString()).X/2;
            spriteBatch.DrawString(Constants.FONT, Player.score.ToString(), new Vector2(centerPosScore, scorePosY), Constants.fontColor2);
            //Föremål kvar att plocka
            spriteBatch.DrawString(Constants.FONT_BIG, strItemsLeft, strItemsLeftPos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT_BIG, itemsLeftToCollect.ToString(), new Vector2(strItemsLeftPos.X + Constants.FONT_BIG.MeasureString(strItemsLeft).X, strItemsLeftPos.Y), Constants.fontColor2);
            //Tid kvar
            spriteBatch.DrawString(Constants.FONT_BIG, strTime, strTimePos, Constants.fontColor);
            int timeLeft = (int)Level.timeLeft; //För att avrunda till heltal
            spriteBatch.DrawString(Constants.FONT_BIG, timeLeft.ToString(), new Vector2(strTimePos.X + Constants.FONT_BIG.MeasureString(strTime).X, strTimePos.Y), Constants.fontColor2);
            //Spelets titel
            spriteBatch.DrawString(Constants.FONT, strTitle, strTitlePos, Constants.fontColor);
            spriteBatch.DrawString(Constants.FONT, strTitleVersion, new Vector2(strTitlePos.X + Constants.FONT.MeasureString(strTitle).X, strTitlePos.Y), Constants.fontColor2);
            //Ammo
            spriteBatch.DrawString(Constants.FONT, "1", new Vector2(330, 20), Color.Gold);
            spriteBatch.Draw(Textures.hamburgare, new Vector2(340, 20), Color.White);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, "3", new Vector2(370, 16), Color.White);

            spriteBatch.DrawString(Constants.FONT, "2", new Vector2(450, 20), Constants.fontColor);
            spriteBatch.Draw(Textures.kebab, new Vector2(470, 17), Color.White * 0.5f);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, "3", new Vector2(497, 16), Color.White * 0.5f);

            spriteBatch.DrawString(Constants.FONT, "3", new Vector2(330, 60), Constants.fontColor);
            spriteBatch.Draw(Textures.bottle, new Vector2(340, 40), Color.White * 0.5f);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, "0", new Vector2(370, 56), Color.Red * 0.5f);

            spriteBatch.DrawString(Constants.FONT, "4", new Vector2(450, 60), Constants.fontColor);
            spriteBatch.Draw(Textures.GUIPizza, new Vector2(470, 55), Color.White * 0.5f);
            spriteBatch.DrawString(Constants.FONT_MEDIUM, "1", new Vector2(497, 56), Color.White * 0.5f);

            spriteBatch.End();
        }
        {
            {
                case Player.weaponType.burger:
                    burgerColor = Color.Gold;
                    burgerOpacity = 1f;
                    kebabColor = Color.White;
                    kebabOpacity = 0.5f;
                    bottleColor = Color.White;
                    bottleOpacity = 0.5f;
                    pizzaColor = Color.White;
                    PizzaOpacity = 0.5f;
                    break;
                case Player.weaponType.kebab:

                    break;
                case Player.weaponType.bottle:

                    break;
                case Player.weaponType.pizza:

                    break;
            }
        }
        /// <summary>
        /// Den där bilden som täcker poweruppen
        /// </summary>
        /// <param name="powerUp"></param>
        /// <param name="time"></param>
        public void ShowPowerUpCounter(int powerUp, double time)
        {
            int frameInterval = (int)(time / 12); //tiden i ms delat med antal frames
            activePowerUp = new ActivePowerUpDisplay(powerUpPos, Textures.powerUpTimer, new Rectangle(0, 0, 63, 63), true, 12, frameInterval, powerUp);
        }
        /// <summary>
        /// Ändrar healthBar-texturen om healthBarBlinkTimer är >= 0 så att det ser ut som att den blinkar till
        /// </summary>
        /// <param name="gameTime"></param>
        private void HealthBarBlinking(GameTime gameTime)
        {
            if (healthBarBlinkTimer >= 0)
            {
                healthBarTex = Textures.healthBarRedBlink;
            }
            else
            {
                healthBarTex = Textures.healthBarRed;
            }
            healthBarBlinkTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        }
        /// <summary>
        /// I Player klassen så sätts healthBarBlinking till true när player ökar/minskar health. Det aktiverar timern här så att healthBar blinkar till
        /// </summary>
        public void BlinkHealthBar()
        {
            if (healthBarBlinking) 
            { 
                healthBarBlinkTimer = healthBarBlinkTimerDefault;
                healthBarBlinking = false;
            }
        }
        public void ResetPowerUp()
        {
            activePowerUp.isActive = false;
        }
    }
}
